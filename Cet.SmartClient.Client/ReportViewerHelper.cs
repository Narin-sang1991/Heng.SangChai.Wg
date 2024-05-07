using Cet.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cet.SmartClient.Client
{
    public class ReportViewerHelper
    {
        public static void ShowError(Exception ex)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
            {
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(ex.ToString(), ApplicationLogCategory.General, (int)Priority.High, (int)GeneralApplicationLogEvent.DispatcherUnhandledException, System.Diagnostics.TraceEventType.Error);
                MessageBox.Show(ex.Message);
            }));
        }
    }
}
