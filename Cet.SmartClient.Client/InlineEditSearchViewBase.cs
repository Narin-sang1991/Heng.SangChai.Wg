using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cet.Core.Logging;
using Cet.Core;
using Telerik.Windows.Controls;
using System.ServiceModel;
using Telerik.Windows.Controls.GridView;
using Microsoft.Practices.Unity;
using Cet.SmartClient.Client.Resources;

namespace Cet.SmartClient.Client
{
    public class InlineEditSearchViewBase : SearchViewBase //where SearchVMType : SearchVMBase where EditorVMType : EditorVMBase
    {
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

        /// <summary>
        /// This is going to Save row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnRowValidating(object sender, GridViewRowValidatingEventArgs e)
        {
            try
            {
                EditorVMBase editor = ((GridViewRow)e.Row).Item as EditorVMBase;
                editor.Save();
                e.IsValid = true;
            }
            catch (FaultException<Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResults> e1)
            {
                e.IsValid = false;
                e.ValidationResults.Add(e1.Detail);
            }
            catch (FaultException<ApplicationSecurityException> e2)
            {
                e.IsValid = false;
                e.ValidationResults.Add(new GridViewCellValidationResult() { ErrorMessage = "Permission denied." });
            }
            catch (FaultException<DataValidationException> e3)
            {
                e.IsValid = false;
                e.ValidationResults.Add(new GridViewCellValidationResult() { ErrorMessage = e3.Detail.Message });
            }
            catch (FaultException<DomainException> e3)
            {
                e.IsValid = false;
                e.ValidationResults.Add(new GridViewCellValidationResult() { ErrorMessage = e3.Detail.Message });
            }
            catch (FaultException faultException)
            {
                e.IsValid = false;
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(faultException.ToString(), ApplicationLogCategory.General, (int)Priority.High, (int)GeneralApplicationLogEvent.DispatcherUnhandledException, System.Diagnostics.TraceEventType.Error);
                e.ValidationResults.Add(new GridViewCellValidationResult() { ErrorMessage = Messages.MSG_UNKNOW_SERVER_ERROR });
            }
            catch
            {
                e.IsValid = false;
                throw;
            }
        }

        protected virtual void OnRowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            if (e.EditOperationType == GridViewEditOperationType.Insert && e.EditAction == GridViewEditAction.Cancel) return;
            SearchVM.Search();
        }

    }
}
