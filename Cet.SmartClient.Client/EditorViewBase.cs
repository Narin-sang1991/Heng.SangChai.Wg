using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Data;
using System.ServiceModel;
using Cet.Core.Logging;
using System.ComponentModel;
using System.Windows;
using Cet.Core;
using Cet.SmartClient.Client.Resources;

namespace Cet.SmartClient.Client
{
    public class EditorViewBase : UnityViewBase 
    {
        protected virtual void OnDeletingItem(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show(Messages.MSG_AREYOUSURE, Messages.MSG_CONFIRM_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question)
                != MessageBoxResult.Yes)
            {
                e.Cancel = true;
                return;
            }
            try
            {
                EditorVMBase editor = ((RadDataForm)sender).CurrentItem as EditorVMBase;
                if (editor != null)
                    editor.Remove();
            }
            catch (FaultException<ApplicationSecurityException> e2)
            {
                e.Cancel = true;
                MessageBox.Show(Messages.MSG_PERMISSION_DENIED, Messages.MSG_ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FaultException<DataValidationException> e3)
            {
                e.Cancel = true;
                MessageBox.Show(e3.Detail.Message, Messages.MSG_ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                ((RadDataForm)sender).ValidationSummary.Errors.Clear();
                ((RadDataForm)sender).ValidationSummary.Errors.Add(new ErrorInfo() { ErrorContent = ex.Message });
            }
        }

        protected void OnEditEnding(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndingEventArgs e)
        {
            if (e.EditAction != Telerik.Windows.Controls.Data.DataForm.EditAction.Commit) return;
            try
            {
                EditorVMBase editor = ((RadDataForm)sender).CurrentItem as EditorVMBase;
                editor.Save();
            }
            catch (FaultException<Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResults> e1)
            {
                e.Cancel = true;
                StringBuilder errorMsg = new StringBuilder();
                foreach (Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult result in e1.Detail)
                {
                    errorMsg.AppendLine(result.Message);
                }

                MessageBox.Show(errorMsg.ToString(), Messages.MSG_ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);                
            }
            catch (FaultException<DataValidationException> e3)
            {
                e.Cancel = true;
                MessageBox.Show(e3.Detail.Message, Messages.MSG_ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FaultException<DomainException> e3)
            {
                e.Cancel = true;
                MessageBox.Show(e3.Detail.Message, Messages.MSG_ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FaultException<ApplicationSecurityException> e2)
            {
                e.Cancel = true;
                MessageBox.Show(Messages.MSG_PERMISSION_DENIED, Messages.MSG_ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FaultException faultException)
            {
                e.Cancel = true;
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(faultException.Data.ToString(), ApplicationLogCategory.General, (int)Priority.High, (int)GeneralApplicationLogEvent.DispatcherUnhandledException, System.Diagnostics.TraceEventType.Error);
                MessageBox.Show(faultException.Data.ToString(), Messages.MSG_ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                e.Cancel = true;
                throw;
            }
        }

        protected void OnCurrentItemChanged(object sender, EventArgs e)
        {
            if (((RadDataForm)sender).CurrentItem == null) return;

            EditorVMBase editorVM = ((RadDataForm)sender).CurrentItem as EditorVMBase;

            if (!editorVM.HasOriginalSource)
                ((RadDataForm)sender).BeginEdit();
        }

        protected void OnEditEnded_ReloadWhenCommit(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e) 
        {
            if (e.EditAction != Telerik.Windows.Controls.Data.DataForm.EditAction.Commit) return;
            EditorVMBase editorVM = ((RadDataForm)sender).CurrentItem as EditorVMBase;
            if (editorVM == null) return;
            editorVM.ReloadAsync();
        }

    }
}
