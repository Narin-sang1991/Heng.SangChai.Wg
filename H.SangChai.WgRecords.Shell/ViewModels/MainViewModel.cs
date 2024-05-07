using Cet.SmartClient.Client;
using H.SangChai.WgRecords.Shell.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace H.SangChai.WgRecords.Shell.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private IUnityContainer container;
        private IRegionManager regionManager;

        private ObservableCollection<MenuItem> menuList;
        public ObservableCollection<MenuItem> MenuList
        {
            get { return menuList; }
        }

        private string ouName;
        public string OuName
        {
            get { return ouName; }
            set
            {
                ouName = value;
                OnPropertyChanged("OuName");
            }
        }

        private string profileName;
        public string ProfileName
        {
            get { return profileName; }
            set
            {
                profileName = value;
                OnPropertyChanged("ProfileName");
            }
        }

        public MainViewModel(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
            this.menuList = new ObservableCollection<MenuItem>();
            this.container.RegisterInstance("MenuList", menuList);
            this.OuName = "Heng-Sang-Chai";
        }

        public ICommand CloseCommand
        {
            get
            {
                return this.container.Resolve<CloseCommand>();
            }
        }


    }
}
