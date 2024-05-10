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

        public void LoadAnonymousMenu()
        {
            ObservableCollection<MenuItem> menuList = this.container.Resolve<ObservableCollection<MenuItem>>("MenuList");
            menuList.Clear();

            CanExecuteMenuList = this.container.Resolve<ObservableCollection<Guid>>("CanExecuteMenuList");
            CanExecuteMenuList.Clear();
            #region Manual Menu
            var menuData = new List<MenuData01>();
            menuData.Add(new MenuData01
            {
                Code = "FileGroup",
                Name = "ไฟล์",
                ResourceUID = Guid.Parse("EDE84469-46D2-4B91-9ABB-D5D86554B9DC"),
                Ordinary = 1,
            });
            menuData.Add(new MenuData01
            {
                Code = "WgRecord",
                Name = "บันทึกน้ำหนัก",
                ParentCode = "FileGroup",
                ResourceUID = Guid.Parse("E80DD6BC-707A-4218-827B-6271A474B02E"),
                Command = "WgRecord",
                Ordinary = 2,
            });
            //menuData.Add(new MenuData01
            //{
            //    Code = "Language",
            //    Name = "Language",
            //    ParentCode = "FileGroup",
            //    ResourceUID = Guid.Parse("06D4E712-4352-4722-82B8-49CBB68DF569"),
            //    Ordinary = 2,
            //});
            //menuData.Add(new MenuData01
            //{
            //    Code = "ChangeLanguageEN",
            //    Name = "EN",
            //    ParentCode = "Language",
            //    ResourceUID = Guid.Parse("53DD6BD8-BE55-4CC2-B1D5-3FA7A08017D5"),
            //    Command = "LanguageENCommand",
            //    Ordinary = 1,
            //});
            //menuData.Add(new MenuData01
            //{
            //    Code = "ChangeLanguageTH",
            //    Name = "TH",
            //    ParentCode = "Language",
            //    ResourceUID = Guid.Parse("E7954B72-FC42-450E-9128-62E7E48B514B"),
            //    Command = "LanguageTHCommand",
            //    Ordinary = 2,
            //});
            menuData.Add(new MenuData01
            {
                Code = "About",
                Name = "เกี่ยวกับ",
                ParentCode = "FileGroup",
                ResourceUID = MenuResources.About,
                Command = "About",
                Ordinary = 3,
            });
            #endregion

            IList<MenuData01> parentList = menuData.Where(t => t.ParentCode == null).ToList<MenuData01>();
            for (int i = 0; i < parentList.Count; i++)
            {
                MenuItem item = new MenuItem() { Header = parentList[i].Name };
                if (!string.IsNullOrEmpty(parentList[i].Command))
                {
                    item.Command = container.Resolve<MenuCommand>(parentList[i].Command);
                }
                var parentCode = parentList[i].Code;
                AddChildMenu(menuData, item, parentCode);
                menuList.Add(item);

                var childs = menuData.Where(t => t.ParentCode != null && t.ParentCode.Equals(parentCode, StringComparison.CurrentCultureIgnoreCase)).ToList();
                foreach (var child in childs) CanExecuteMenuList.Add(child.ResourceUID);
            }
        }

        private void AddChildMenu(IList<MenuData01> list, MenuItem parentMenu, string parentCode)
        {
            var items = list.Where(t => t.ParentCode != null && t.ParentCode.Equals(parentCode, StringComparison.CurrentCultureIgnoreCase)).ToList();
            foreach (MenuData01 data in items)
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
