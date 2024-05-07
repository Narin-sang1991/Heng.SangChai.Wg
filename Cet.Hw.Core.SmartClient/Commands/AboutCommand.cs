using Cet.Hw.Core.AppServiceContract;
using Cet.Hw.Core.SmartClient.ViewModels;
using Cet.SmartClient.Client;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Hw.Core.SmartClient.Commands
{
    public class AboutCommand : MenuCommand
    {
        private readonly IUnityContainer container;

        public AboutCommand(IUnityContainer container)
            : base(container, MenuResources.About)
        {
            this.container = container;
        }

        public override void Execute(object parameter)
        {
            Open();
        }

        private void Open()
        {
            AboutManagerView view = this.container.Resolve<AboutManagerView>();
            AboutManagerVM viewModel = this.container.Resolve<AboutManagerVM>();
            view.DataContext = viewModel;
            view.ShowDialog();
        }

        #region ICommand Members

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        #endregion
    }
}