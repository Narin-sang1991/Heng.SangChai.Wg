using Cet.Core;
using Cet.Core.Data;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Cet.Hw.Core.AppServiceContract
{
    [ServiceContract]
    public interface IUserService
    {

        [OperationContract]
        [FaultContract(typeof(ApplicationSecurityException))]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ValidationResults))]
        [FaultContract(typeof(DataValidationException))]
        [FaultContract(typeof(UserNotLoginException))]
        Guid Save(UserData data);

        [OperationContract]
        [FaultContract(typeof(ApplicationSecurityException))]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ValidationResults))]
        [FaultContract(typeof(DataValidationException))]
        [FaultContract(typeof(UserNotLoginException))]
        void Remove(Guid userId);

        [OperationContract]
        [FaultContract(typeof(ApplicationSecurityException))]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ValidationResults))]
        [FaultContract(typeof(DataValidationException))]
        [FaultContract(typeof(UserNotLoginException))]
        IList<UserData> Find(UserCriteria01 criteria, SortingCriteria sortingCriteria, PagingCriteria pagingCriteria);

        [OperationContract]
        [FaultContract(typeof(ApplicationSecurityException))]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ValidationResults))]
        [FaultContract(typeof(DataValidationException))]
        [FaultContract(typeof(UserNotLoginException))]
        int Count(UserCriteria01 criteria);

        [OperationContract]
        [FaultContract(typeof(ApplicationSecurityException))]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ValidationResults))]
        [FaultContract(typeof(DataValidationException))]
        [FaultContract(typeof(UserNotLoginException))]
        IList<BaseStdData> FindParent(Guid parentId);

    }
}
