using Cet.SmartClient.Client;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Docking;
namespace H.SangChai.WgRecords.Shell.Commands
{
    public class CloseCommand : ICommand
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;

        public CloseCommand(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public delegate void CloseTabHandler();

        public void Execute(object parameter)
        {
            StateChangeEventArgs e = parameter as StateChangeEventArgs;

            foreach (RadPane pane in e.Panes)
            {
                EditableContainerBase ecb = pane.DataContext as EditableContainerBase;
                if (ecb != null)
                {
                    if (ecb.HasEditingChilds)
                    {
                        var dr = MessageBox.Show(Cet.SmartClient.Client.Resources.Messages.MSG_CONFIRM, Cet.SmartClient.Client.Resources.Messages.MSG_CONFIRM_CAPTION, MessageBoxButton.YesNo);
                        if (dr == MessageBoxResult.No)
                        {
                            e.Handled = true;
                            break;
                        }
                    }
                    ecb.Dispose();
                }
            }

            if (e.Handled) return;

            IRegion region = regionManager.Regions["ContentRegion"];

            foreach (RadPane pane in e.Panes)
            {
                pane.DataContext = null;
                region.Remove(pane);
            }

        }
        #endregion
    }
}
