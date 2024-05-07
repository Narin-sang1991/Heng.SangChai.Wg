using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Telerik.Windows.Controls;
using Cet.SmartClient.Client.Resources;

namespace Cet.SmartClient.Client
{
    public class DocSearchViewBase : UnityViewBase
    {
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


        protected void OnDeleting(object sender, GridViewDeletingEventArgs e)
        {
            if (MessageBox.Show(Messages.MSG_AREYOUSURE, Messages.MSG_CONFIRM_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question)
                != MessageBoxResult.Yes)
            {
                e.Handled = true;
                e.Cancel = true;
                return;
            }

            foreach (Object o in e.Items)
                (DataContext as DocEditorVMBase).RemoveChildNode(o);

            e.Handled = true;
        }
        protected virtual void OnAddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            var grid = e.OwnerGridViewItemsControl;
            grid.CurrentColumn = grid.Columns[1];
            grid.UnselectAll();
            Dispatcher.BeginInvoke(new Action(() =>
            {
                ((RadGridView)sender).Items.MoveToLastPage();
            }));
        }

    }

}
