using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Cet.Core;

namespace Cet.Hw.Core.AppServiceContract
{
    [ServiceContract]
    public interface IAuthenticationService
    {
        [OperationContract]
        [FaultContract(typeof(DataValidationException))]
        [FaultContract(typeof(ApplicationSecurityException))]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ValidationResults))]
        [FaultContract(typeof(UserNotLoginException))]
        UserProfile GetCurrentUserProfile(bool isThrowEx = true);

        [OperationContract]
        [FaultContract(typeof(DataValidationException))]
        [FaultContract(typeof(ApplicationSecurityException))]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ValidationResults))]
        [FaultContract(typeof(UserNotLoginException))]
        void Logout();
    }
}
