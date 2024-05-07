using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cet.Core.Data;
using Telerik.Windows.Controls;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using System.ServiceModel;
using Cet.Core;
using Cet.SmartClient.Client.Resources;
using System.Threading.Tasks;

namespace Cet.SmartClient.Client
{
    //public class SearchViewBase<SearchVMType> : SearchViewBase where SearchVMType : SearchVMBase
    //{
    //    public SearchVMType SearchVM
    //    {
    //        get { return this.DataContext as SearchVMType; }
    //    }
    //}

    public class SearchViewBase : UnityViewBase
    {
        public SearchVMBase SearchVM
        {
            get { return this.DataContext as SearchVMBase; }
        }

        protected void OnCopyingCellClipboardContent(object sender, Telerik.Windows.Controls.GridViewCellClipboardEventArgs e)
        {
            var grid = sender as RadGridView;
            if (grid.CurrentCell != null && grid.CurrentCell.Column == e.Cell.Column)
            {
                e.Cancel = false;
            }
            else
                e.Cancel = true;
        }

        protected async void OnSortingAsync(object sender, Telerik.Windows.Controls.GridViewSortingEventArgs e)
        {
            SearchVMBase viewModel = SearchVM;
            if (viewModel == null) return;
            if (e.OldSortingState == SortingState.None)
                e.NewSortingState = SortingState.Ascending;
            else if (e.OldSortingState == SortingState.Ascending)
                e.NewSortingState = SortingState.Descending;
            else
                e.NewSortingState = SortingState.None;
            viewModel.SortingCriteria.Clear();
            viewModel.SortingCriteria.Add(new SortBy() { Name = e.Column.SortMemberPath, Direction = e.NewSortingState == SortingState.None || e.NewSortingState == SortingState.Ascending ? SortDirection.ASC : SortDirection.DESC });
            e.Cancel = true; 
            
            await viewModel.SearchAsync();
            
            foreach(var data in e.DataControl.Columns)
            {
                if (data != e.Column && data.SortingState != SortingState.None)
                    data.SortingState = SortingState.None;
            }
        }

        protected void OnPageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            //SearchVMType viewModel = SearchVM;
            //if (viewModel != null)
            //    viewModel.Search();
        }

        protected void OnSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            SearchVMBase viewModel = SearchVM;

            RadGridView radGridview = sender as Telerik.Windows.Controls.RadGridView;

            if (viewModel == null) return;

            if (!viewModel.HasEditingChilds && radGridview.SelectedItems.Count > 0)
            {
                viewModel.HasSelectedItem = true;
                viewModel.AllowDelete = true;
            }
            else
            {
                viewModel.HasSelectedItem = false;
                viewModel.AllowDelete = false;
            }

            SelectedItems = radGridview.SelectedItems;
        }

        protected void OnSelectionChanging(object sender, SelectionChangingEventArgs e)
        {
            SearchVMBase viewModel = SearchVM;

            if (viewModel.HasEditingChilds)
                e.Cancel = true;
        }

        public ObservableCollection<object> SelectedItems { get; private set; }

        protected void OnDeleting(object sender, GridViewDeletingEventArgs e)
        {
            if (MessageBox.Show(Messages.MSG_AREYOUSURE, Messages.MSG_CONFIRM_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question)
                != MessageBoxResult.Yes)
            {
                e.Cancel = true;
                e.Handled = true;
                return;
            }
            //ถ้าเลือกลบมากกว่า 1 item จะ loop ลบทีล่ะตัว
            foreach (var item in e.Items)
            {
                try
                {
                    //ลบทีละตัว 
                    SearchVM.RemoveItem(item);
                }
                catch (FaultException<ApplicationSecurityException> e2)
                {
                    MessageBox.Show(Messages.MSG_PERMISSION_DENIED, Messages.MSG_ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (FaultException<DataValidationException> e3)
                {
                    MessageBox.Show(e3.Detail.Message, Messages.MSG_ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Messages.MSG_ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            SearchVM.Search();  //remove จนเสร็จแล้ว search ใหม่
            e.Cancel = false;
            e.Handled = true;
        }

        protected async void OnDeletingAsync(object sender, GridViewDeletingEventArgs e)
        {
            if (MessageBox.Show(Messages.MSG_AREYOUSURE, Messages.MSG_CONFIRM_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question)
                != MessageBoxResult.Yes)
            {
                e.Cancel = true;
                e.Handled = true;
                return;
            }

            e.Cancel = false;
            e.Handled = true;

            //ถ้าเลือกลบมากกว่า 1 item จะ loop ลบทีล่ะตัว
            foreach (var item in e.Items)
            {
                try
                {
                    //ลบทีละตัว 
                    await SearchVM.RemoveItemAsync(item);
                }
                catch (FaultException<ApplicationSecurityException> e2)
                {
                    MessageBox.Show(Messages.MSG_PERMISSION_DENIED, Messages.MSG_ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (FaultException<Cet.Core.DataValidationException> e3)
                {
                    MessageBox.Show(e3.Detail.Message, Messages.MSG_ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Messages.MSG_ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            await SearchVM.SearchAsync();  //remove จนเสร็จแล้ว search ใหม่
        }

    }

}
 