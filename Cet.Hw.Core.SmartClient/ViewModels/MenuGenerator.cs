using Cet.Hw.Core.AppServiceContract;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Cet.Core.Data;
using Cet.SmartClient.Client;
using Cet.Hw.Core.SmartClient.Commands;
using Cet.Core.Logging;

namespace Cet.Hw.Core.SmartClient.ViewModels
{
    public class MenuGenerator
    {
        private IUnityContainer container;

        private ObservableCollection<Guid> canExecuteMenuList;
        public ObservableCollection<Guid> CanExecuteMenuList
        {
            get { return canExecuteMenuList; }
            set
            {
                canExecuteMenuList = value;
                //OnPropertyChanged("CanExecuteMenuList");
            }
        }

        private ObservableCollection<Guid> canManageMenuList;
        public ObservableCollection<Guid> CanManageMenuList
        {
            get { return canManageMenuList; }
            set
            {
                canManageMenuList = value;
                //OnPropertyChanged("CanExecuteMenuList");
            }
        }

        public MenuGenerator(IUnityContainer container)
        {
            this.container = container;
        }

        public void LoadMenu()
        {
            ObservableCollection<MenuItem> menuList = this.container.Resolve<ObservableCollection<MenuItem>>("MenuList");
            menuList.Clear();
            var menuService = this.container.Resolve<IMenuService>();
            IList<MenuData01> list = menuService.Find(new MenuCriteria01() { IsHidden = false }, null, null).OrderBy(t => t.Ordinary).ToList<MenuData01>();
            IList<MenuData01> parentList = list.Where(t => t.ParentCode == null).ToList<MenuData01>();
            for (int i = 0; i < parentList.Count; i++)
            {
                MenuItem item = new MenuItem() { Header = parentList[i].Name };
                if (!string.IsNullOrEmpty(parentList[i].Command))
                {
                    item.Command = container.Resolve<MenuCommand>(parentList[i].Command);
                }
                AddChildMenu(list, item, parentList[i].Code);
                menuList.Add(item);
            }
        }

        private void AddChildMenu(IList<MenuData01> list, MenuItem parentMenu, string parentCode)
        {

            foreach (MenuData01 data in list.Where(t => t.ParentCode == parentCode))
            {
                try
                {
                    MenuItem menuItem = new MenuItem() { Header = data.Name };
                    if (!string.IsNullOrEmpty(data.Command))
                    {
                        menuItem.Command = container.Resolve<MenuCommand>(data.Command);
                        if (menuItem.Command is ChangeLanguageCommand)
                        {
                            ChangeLanguageCommand langCommand = menuItem.Command as ChangeLanguageCommand;
                            if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == langCommand.LanguageName)
                                menuItem.IsChecked = true;
                        }
                    }
                    parentMenu.Items.Add(menuItem);
                    AddChildMenu(list, menuItem, data.Code);
                }
                catch (Exception ex)
                {
                    Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write("Load menu => " + ex.ToString(), ApplicationLogCategory.General, (int)Priority.High, (int)GeneralApplicationLogEvent.DispatcherUnhandledException, System.Diagnostics.TraceEventType.Error);
                }
            }
        }

        public void GetExecutePermission()
        {
            CanExecuteMenuList = this.container.Resolve<ObservableCollection<Guid>>("CanExecuteMenuList");
            CanExecuteMenuList.Clear();
            IMenuService menuService = this.container.Resolve<IMenuService>();

            IList<Guid> result = menuService.GetAccessibleMenuList();
            foreach (Guid uid in result)
                CanExecuteMenuList.Add(uid);
        }

        public void GetManagePermission()
        {
            CanManageMenuList = this.container.Resolve<ObservableCollection<Guid>>("CanManageMenuList");
            CanManageMenuList.Clear();
            IMenuService menuService = this.container.Resolve<IMenuService>();

            IList<Guid> result = menuService.GetManageMenuList();
            foreach (Guid uid in result)
                CanManageMenuList.Add(uid);
        }
    }

}
