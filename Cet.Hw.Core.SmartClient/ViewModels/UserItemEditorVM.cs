using Cet.Hw.Core.AppServiceContract;
using Cet.SmartClient.Client;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Hw.Core.SmartClient.ViewModels
{
    public class UserItemEditorVM : EditorVMBase<UserData>
    {

        #region Properties
        private Guid? id;
        public Guid? Id
        {
            get { return id; }
            private set { id = value; }
        }

        private Guid? groupId;
        public Guid? GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string login;
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                OnPropertyChanged("IsActive");
            }
        }

        private DateTimeOffset? createdDate;
        public DateTimeOffset? CreatedDate
        {
            get { return createdDate; }
            private set
            {
                createdDate = value;
                OnPropertyChanged("CreatedDate");
            }
        }

        private DateTimeOffset? updatedDate;
        public DateTimeOffset? UpdatedDate
        {
            get { return updatedDate; }
            private set
            {

                updatedDate = value;
                OnPropertyChanged("UpdatedDate");
            }
        }


        #endregion

        public UserItemEditorVM()
            : base()
        { }

        public UserItemEditorVM(IUnityContainer container)
            : base(container)
        { }

        protected override void SaveInternal()
        {
            var service = Container.Resolve<IUserService>();
            var data = new UserData();
            this.SaveOriginalSource(data);
            this.Id = service.Save(data);
        }

        protected override void LoadOriginalSource(UserData originalSource)
        {
            this.Id = originalSource.UserId;
            this.GroupId = originalSource.ParentId;
            this.Name = originalSource.Name;
            this.Login = originalSource.Login;
            this.Password = originalSource.Password;
            this.IsActive = originalSource.IsActive;
            this.CreatedDate = originalSource.CreatedDate;
            this.UpdatedDate = originalSource.UpdatedDate;
        }

        protected override void SaveOriginalSource(UserData originalSource)
        {
            originalSource.UserId = this.Id.HasValue ? this.Id.Value : Guid.Empty;
            originalSource.ParentId = this.GroupId;
            originalSource.Name = this.Name;
            originalSource.Login = this.Login;
            originalSource.Password = this.Password;
            originalSource.IsActive = this.IsActive;
        }


        protected override void RemoveInternal()
        {
            var service = Container.Resolve<IUserService>();
            if (this.Id.HasValue)
                service.Remove(this.Id.Value);
        }
    }
}
