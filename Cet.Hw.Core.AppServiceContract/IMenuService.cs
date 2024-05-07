using Cet.Core;
using Cet.Core.Data;
using Cet.Hw.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Hw.Core.AppServiceContract
{
    [ServiceContract]
    public interface IMenuService
    {
        [OperationContract]
        [FaultContract(typeof(DataValidationException))]
        [FaultContract(typeof(ApplicationSecurityException))]
        [FaultContract(typeof(UserNotLoginException))]
        IList<MenuData01> Find(MenuCriteria01 criteria, SortingCriteria sortingCriteria, PagingCriteria pagingCriteria);

        [OperationContract]
        [FaultContract(typeof(DataValidationException))]
        [FaultContract(typeof(ApplicationSecurityException))]
        [FaultContract(typeof(UserNotLoginException))]
        IList<Guid> GetAccessibleMenuList();

        [OperationContract]
        [FaultContract(typeof(DataValidationException))]
        [FaultContract(typeof(ApplicationSecurityException))]
        [FaultContract(typeof(UserNotLoginException))]
        IList<Guid> GetManageMenuList();
    }

}
