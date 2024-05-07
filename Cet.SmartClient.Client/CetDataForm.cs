using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows;
using Cet.SmartClient.Client.Resources;
using Telerik.Windows.Controls.Data.DataForm;

namespace Cet.SmartClient.Client
{
    public class CetDataForm : RadDataForm, IWeakEventListener
    {
        public CetDataForm()
        {
            this.CommitButtonContent = Cet.SmartClient.Client.Resources.Messages.OK_BUTTON;
            this.CancelButtonContent = Cet.SmartClient.Client.Resources.Messages.CANCEL_BUTTON;

            CommandBinding moveToNextCommand = new CommandBinding(RadDataFormCommands.MoveCurrentToNext, MoveCurrentToNextCommandHandler, MoveCurrentToNextCanExecuteHandler);
            this.CommandBindings.Add(moveToNextCommand);

            CommandBinding moveToPreviousCommand = new CommandBinding(RadDataFormCommands.MoveCurrentToPrevious, MoveCurrentToPreviousCommandHandler, MoveCurrentToPreviousCanExecuteHandler);
            this.CommandBindings.Add(moveToPreviousCommand);

            CommandBinding moveToFirstCommand = new CommandBinding(RadDataFormCommands.MoveCurrentToFirst, MoveCurrentToFirstCommandHandler, MoveCurrentToPreviousCanExecuteHandler);
            this.CommandBindings.Add(moveToFirstCommand);

            CommandBinding moveToLastCommand = new CommandBinding(RadDataFormCommands.MoveCurrentToLast, MoveCurrentToLastCommandHandler, MoveCurrentToNextCanExecuteHandler);
            this.CommandBindings.Add(moveToLastCommand);

            CommandBinding editCommand = new CommandBinding(RadDataFormCommands.BeginEdit, BeginEditExecute, CanExecuteBeginEdit);
            this.CommandBindings.Add(editCommand);

            CommandBinding deleteCommand = new CommandBinding(RadDataFormCommands.Delete, DeleteExecute, CanExecuteDelete);
            this.CommandBindings.Add(deleteCommand);

            CommandBinding commitEditCommand = new CommandBinding(RadDataFormCommands.CommitEdit, CommitEditExecute, CanExecuteCommitEdit);
            this.CommandBindings.Add(commitEditCommand);

            CommandBinding cancelEditCommand = new CommandBinding(RadDataFormCommands.CancelEdit, CancelEditExecute, CanExecuteCancelEdit);
            this.CommandBindings.Add(cancelEditCommand);

            WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.AddEventListener(this);
            this.Unloaded -= new RoutedEventHandler(this.OnUnloaded);
        }

        #region Navigator

        // Executed event handler.
        private void MoveCurrentToNextCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            FsListCollectionViewBase items = this.ItemsSource as FsListCollectionViewBase;

            if (items == null) return;

            items.MoveCurrentToNext();
            e.Handled = true;
        }

        private void MoveCurrentToPreviousCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            FsListCollectionViewBase items = this.ItemsSource as FsListCollectionViewBase;

            if (items == null) return;

            items.MoveCurrentToPrevious();
            e.Handled = true;
        }

        private void MoveCurrentToFirstCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            FsListCollectionViewBase items = this.ItemsSource as FsListCollectionViewBase;

            if (items == null) return;

            items.MoveCurrentToFirst();
            e.Handled = true;
        }

        private void MoveCurrentToLastCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            FsListCollectionViewBase items = this.ItemsSource as FsListCollectionViewBase;

            if (items == null) return;

            items.MoveCurrentToLast();
            e.Handled = true;
        }

        // CanExecute event handler.
        private void MoveCurrentToNextCanExecuteHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            FsListCollectionViewBase items = this.ItemsSource as FsListCollectionViewBase;

            if (items == null)
            {
                e.ContinueRouting = true;
                return;
            }

            if (this.Mode == Telerik.Windows.Controls.Data.DataForm.RadDataFormMode.Edit || this.Mode == Telerik.Windows.Controls.Data.DataForm.RadDataFormMode.AddNew)
                e.CanExecute = false;
            else
                e.CanExecute = items.CanMoveToNext();

            e.Handled = true;
        }

        private void MoveCurrentToPreviousCanExecuteHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            FsListCollectionViewBase items = this.ItemsSource as FsListCollectionViewBase;

            if (items == null)
            {
                e.ContinueRouting = true;
                return;
            }

            if (this.Mode == Telerik.Windows.Controls.Data.DataForm.RadDataFormMode.Edit || this.Mode == Telerik.Windows.Controls.Data.DataForm.RadDataFormMode.AddNew)
                e.CanExecute = false;
            else
                e.CanExecute = items.CanMoveToPrevious();

            e.Handled = true;
        }

        #endregion

        #region Edit Command
        private void BeginEditExecute(object sender, ExecutedRoutedEventArgs e)
        {
            base.BeginEdit();
            e.Handled = true;
        }

        private void CanExecuteBeginEdit(object sender, CanExecuteRoutedEventArgs e)
        {
            EditableContainerBase item = this.CurrentItem as EditableContainerBase;
            if (item == null)
            {
                e.ContinueRouting = true;
                return;
            }

            if (!item.CanEdit)
                e.CanExecute = item.CanEdit;
            else
                e.CanExecute = base.CanBeginEdit;
            e.Handled = true;
        }
        #endregion

        #region Delete Command
        private void DeleteExecute(object sender, ExecutedRoutedEventArgs e)
        {
            EditorVMBase item = this.CurrentItem as EditorVMBase;
            if (MessageBox.Show(Messages.MSG_AREYOUSURE, Messages.MSG_CONFIRM_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question)
                != MessageBoxResult.Yes)
            {
                e.Handled = true;
                return;
            }
            e.Handled = true;
            if (item != null)
                item.Remove();
        }

        private void CanExecuteDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            EditableContainerBase item = this.CurrentItem as EditableContainerBase;
            if (item == null)
            {
                e.ContinueRouting = true;
                return;
            }

            if (item.AllowDelete && item.IsNotEditing)
                e.CanExecute = item.AllowDelete;
            else
                e.CanExecute = base.CanDeleteItems;
            e.Handled = true;
        }

        
        #endregion

        #region Commit Edit Command
        private void CommitEditExecute(object sender, ExecutedRoutedEventArgs e)
        {
            // สังให้ Row ใน GridView ทำการ EndEdit ก่อน เพื่อป้องกันปัญหาการกรอกข้อมูลแล้วข้อมูลไม่อัพเดท
            if (RadGridViewCommands.CommitEdit.CanExecute(null))
                RadGridViewCommands.CommitEdit.Execute(null);

            // เปลี่ยน focus มาอยู่ที่ระดับ DataForm เพื่อป้องกันปัญหาการกรอกข้อมูลแล้วข้อมูลไม่อัพเดท (สำหรับ Element ที่ไม่ได้อยู่ใน GridView เช่น Textbox ที่ต้อง Lostfocus ก่อน)
            this.Focus();

            base.CommitEdit();
        }

        private void CanExecuteCommitEdit(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CanCommitEdit;
        }
        #endregion

        #region Cancel Edit Command
        private void CancelEditExecute(object sender, ExecutedRoutedEventArgs e)
        {
            // สังให้ Row ใน GridView ทำการ EndEdit ก่อน เพื่อป้องกันปัญหาการกรอกข้อมูลแล้วข้อมูลไม่อัพเดท
            // Not nesssary because data will come from LoadOriginalSource in the end
            if (RadGridViewCommands.CancelRowEdit.CanExecute(null))
                RadGridViewCommands.CancelRowEdit.Execute(null);

            // เปลี่ยน focus มาอยู่ที่ระดับ DataForm เพื่อป้องกันปัญหาการกรอกข้อมูลแล้วข้อมูลไม่อัพเดท (สำหรับ Element ที่ไม่ได้อยู่ใน GridView เช่น Textbox ที่ต้อง Lostfocus ก่อน)
            // Not nesssary because data will come from LoadOriginalSource in the end
            this.Focus();

            base.CancelEdit();
        }

        private void CanExecuteCancelEdit(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CanCancelEdit;
            //e.Handled = true;
        }
        #endregion

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (sender is WPFLocalizeExtension.Engine.LocalizeDictionary)
            {
                this.CommitButtonContent = Cet.SmartClient.Client.Resources.Messages.OK_BUTTON;
                this.CancelButtonContent = Cet.SmartClient.Client.Resources.Messages.CANCEL_BUTTON;
            }
            return true;
        }

        #endregion
    }
}
