using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cet.Hw.Core.AppServiceContract;
using Microsoft.Practices.Unity;
using Cet.Hw.Core.Domain;

namespace Cet.Hw.Core.AppService
{
    public class FormAuthenticationService: IFormAuthenticationService
    {
        private IUnityContainer container;

        [Dependency]
        public AuthenticationServiceBase AuthenticationServiceBase { get; set; }

        public FormAuthenticationService(IUnityContainer container)
        {
            this.container = container;
        }

        //public bool CheckPasswordExpire(string loginName)
        //{
        //    var authenService = this.container.Resolve<IDomainAuthenticationService>();
        //    return authenService.CheckPasswordExpire(loginName);
        //}

        public LoginResult Login(string login, string password, string realm = null)
        {
            var authenService = this.container.Resolve<IDomainAuthenticationService>(AuthType.MyAccount.ToString());
            return authenService.Login(login, password, realm);
        }

        public LoginResult Login(string login, string password, AuthType authType, string realm = null)
        {
            var authenService = this.container.Resolve<IDomainAuthenticationService>(authType.ToString());
            return authenService.Login(login, password, realm);
        }

        public LoginResult LoginWithToken(string token, string realm = null)
        {
            var authenService = this.container.Resolve<IDomainAuthenticationService>(AuthType.MyAccount.ToString());
            return authenService.Login(token, realm);
        }

        public LoginResult LoginWithToken(string token, AuthType authType, string realm = null)
        {
            var authenService = this.container.Resolve<IDomainAuthenticationService>(authType.ToString());
            return authenService.Login(token, realm);
        }

        public UserProfile GetCurrentUserProfile(bool isThrowEx = true)
        {
            return AuthenticationServiceBase.GetCurrentUserProfile(isThrowEx);
        }

        public void Logout()
        {
            AuthenticationServiceBase.Logout();
        }
    }
}
