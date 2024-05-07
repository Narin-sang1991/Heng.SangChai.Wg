using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;
using System.Collections.Specialized;

namespace Cet.SmartClient.Client
{
    public class MenuCommand : ICommand
    {
        private IUnityContainer container;
        private Guid guid;

        private ObservableCollection<Guid> canExecuteMenuList;
        public ObservableCollection<Guid> CanExecuteMenuList
        {
            get { return canExecuteMenuList; }
            set { canExecuteMenuList = value; }
        }

        private ObservableCollection<Guid> canManageMenuList;
        public ObservableCollection<Guid> CanManageMenuList
        {
            get { return canManageMenuList; }
            set { canManageMenuList = value; }
        }

        public MenuCommand(IUnityContainer container, Guid guid)
        {
            this.container = container;
            this.guid = guid;
            CanExecuteMenuList = this.container.Resolve<ObservableCollection<Guid>>("CanExecuteMenuList");
            CanManageMenuList = this.container.Resolve<ObservableCollection<Guid>>("CanManageMenuList");
            CanExecuteMenuList.CollectionChanged += new NotifyCollectionChangedEventHandler(CanExecuteMenuList_CollectionChanged);
        }

        void CanExecuteMenuList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, e);
        }

        #region ICommand Members
        public virtual bool CanExecute(object parameter)
        {
            if (CanExecuteMenuList.Contains(guid))
                return true;

            return false;
        }

        public event EventHandler CanExecuteChanged;

        public virtual void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
