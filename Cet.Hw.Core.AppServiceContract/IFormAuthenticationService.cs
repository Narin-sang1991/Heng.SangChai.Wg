using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Cet.Core;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Cet.Hw.Core.AppServiceContract
{
    [ServiceContract]
    public interface IFormAuthenticationService : IAuthenticationService
    {
        [OperationContract(Name = "Login")]
        [FaultContract(typeof(DataValidationException))]
        [FaultContract(typeof(ApplicationSecurityException))]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ValidationResults))]
        [FaultContract(typeof(UserNotLoginException))]
        LoginResult Login(string login, string password, string realm = null);

        [OperationContract(Name = "LoginByAuthType")]
        [FaultContract(typeof(DataValidationException))]
        [FaultContract(typeof(ApplicationSecurityException))]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ValidationResults))]
        [FaultContract(typeof(UserNotLoginException))]
        LoginResult Login(string login, string password, AuthType authType, string realm = null);

        [OperationContract(Name = "LoginWithToken")]
        [FaultContract(typeof(DataValidationException))]
        [FaultContract(typeof(ApplicationSecurityException))]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ValidationResults))]
        [FaultContract(typeof(UserNotLoginException))]
        LoginResult LoginWithToken(string token, string realm = null);

        [OperationContract(Name = "LoginWithTokenByAuthType")]
        [FaultContract(typeof(DataValidationException))]
        [FaultContract(typeof(ApplicationSecurityException))]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ValidationResults))]
        [FaultContract(typeof(UserNotLoginException))]
        LoginResult LoginWithToken(string token, AuthType authType, string realm = null);


    }
}
