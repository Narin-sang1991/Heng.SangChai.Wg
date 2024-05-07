using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;
using System.Collections.ObjectModel;

namespace Cet.Hw.Core.SmartClient
{
    public class CoreModule : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;

        public CoreModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            this.ConfigureContainer();
        }

        protected void ConfigureContainer()
        {
            // for Accessible Menu
            ObservableCollection<Guid> canExecuteMenuList = new ObservableCollection<Guid>();
            this.container.RegisterInstance("CanExecuteMenuList", canExecuteMenuList);

            // for manage program
            ObservableCollection<Guid> canManageMenuList = new ObservableCollection<Guid>();
            this.container.RegisterInstance("CanManageMenuList", canManageMenuList);
        }
    }
}
