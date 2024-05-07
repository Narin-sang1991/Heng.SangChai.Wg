using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cet.Core;
using System.ComponentModel;
using Microsoft.Practices.Prism.Regions;
using Cet.SmartClient.Client;
using Microsoft.Practices.Unity;
using Cet.Hw.Core.AppServiceContract;
using System.Windows;

namespace Cet.Hw.Core.SmartClient.ViewModels
{
    public class LoginVM : ViewModelBase, IEditableObject
    {
        private IUnityContainer container;
        private IRegionManager regionManager;

        public LoginVM() { }
        public LoginVM(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
            this.OULogoURL = container.Resolve<string>("DefaultLogoURL");
        }

        #region Properties
        private string loginName;
        public string LoginName
        {
            get { return loginName; }
            set
            {
                loginName = value;
                OnPropertyChanged("LoginName");
            }
        }

        private string passwordInput;
        public string PasswordInput
        {
            get { return passwordInput; }
            set
            {
                passwordInput = value;
                OnPropertyChanged("PasswordInput");
            }
        }

        private bool isLoadedMenu;
        public bool IsLoadedMenu
        {
            get { return isLoadedMenu; }
            set
            {
                isLoadedMenu = value;
                OnPropertyChanged("IsLoadedMenu");
            }
        }

        private string ouLogoURL;
        public string OULogoURL
        {
            get { return ouLogoURL; }
            set { ouLogoURL = value; }
        }

        #endregion

        public LoginResultCode CheckLogin()
        {
            return CheckLogin(this.LoginName, this.PasswordInput);
        }

        protected LoginResultCode CheckLogin(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return LoginResultCode.Invalid;
            //else if (SelectedOU == null)
            //    return LoginResultCode.RequiredOU;

            IFormAuthenticationService service = this.container.Resolve<IFormAuthenticationService>();
            LoginResult result = service.Login(login, password);

            if (result.LoginResultCode == LoginResultCode.Success)
            {
                this.container.RegisterType<MessageHeaderInserterBase, SecurityTokenMessageHeaderInserter>("SecurityTokenMessageHeaderInserter");
                this.container.RegisterInstance<UserProfileToken>(new UserProfileToken(result.Token));
                //if (service.CheckPasswordExpire(login))
                //{
                //    MessageBox.Show("Please Change Password", Messages.MSG_WARNING_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                //    ChangePasswordView view = this.container.Resolve<ChangePasswordView>();
                //    ChangePasswordVM viewModel = this.container.Resolve<ChangePasswordVM>();
                //    view.DataContext = viewModel;
                //    if (view.ShowDialog() == false)
                //        result.LoginResultCode = LoginResultCode.PasswordExpire;
                //}

                MenuGenerator menuGenerator = this.container.Resolve<MenuGenerator>();

                if (!IsLoadedMenu)
                    menuGenerator.LoadMenu();
                menuGenerator.GetExecutePermission();
                menuGenerator.GetManagePermission();
                UserProfile profile = service.GetCurrentUserProfile(false);
                this.container.RegisterInstance<UserProfile>("current", profile);

            }

            return result.LoginResultCode;
        }


        #region IEditableObject Members

        public void BeginEdit()
        {
            //throw new NotImplementedException();
        }

        public void CancelEdit()
        {
            //throw new NotImplementedException();
        }

        public void EndEdit()
        {
            //throw new NotImplementedException();
        }

        #endregion

    }
}
