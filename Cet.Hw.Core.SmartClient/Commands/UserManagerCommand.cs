using Cet.Hw.Core.AppServiceContract;
using Cet.Hw.Core.SmartClient.ViewModels;
using Cet.SmartClient.Client;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using WPFLocalizeExtension.Extensions;

namespace Cet.Hw.Core.SmartClient.Commands
{
    public class UserManagerCommand : MenuCommand
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;

        public UserManagerCommand(IUnityContainer container, IRegionManager regionManager)
            : base(container, MenuResources.UserManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public override void Execute(object parameter)
        {
            IRegion region = regionManager.Regions["ContentRegion"];
            string viewName = "UserManagerView";
            object view = region.GetView(viewName);

            if (view == null)
            {
                var usercontrol = this.container.Resolve<UserManagerView>();
                var viewModel = this.container.Resolve<UserManagerVM>();
                viewModel.Header = new LocTextExtension() { Key = "DISPLAY_HEADER", Dict = "UserManagerView", Assembly = "Cet.Hw.Core.SmartClient" };
                usercontrol.DataContext = viewModel;
                view = region.AddView(usercontrol, viewName);
            }

            region.Activate(view);
        }

    }
}