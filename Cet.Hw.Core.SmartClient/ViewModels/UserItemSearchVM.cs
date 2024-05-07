using Cet.Core.Data;
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
    public class UserItemSearchVM : SearchVMBase<UserItemEditorVM, UserData, UserCriteria01>
    {
        public UserItemSearchVM(IUnityContainer container)
            : base(container)
        {
            this.PageSize = 20;
            this.PageIndex = 0;
        }

        protected override int CountItemsInternal(UserCriteria01 criteria)
        {
            var serviceClient = this.Container.Resolve<IUserService>();
            return serviceClient.Count(criteria);
        }
        protected override IList<UserData> SearchInternal(UserCriteria01 criteria, SortingCriteria sortingCriteria, PagingCriteria pagingCriteria)
        {
            var serviceClient = this.Container.Resolve<IUserService>();
            return serviceClient.Find(criteria, sortingCriteria, pagingCriteria);
        }

        protected override void PrepareDefaultSortingCriteria(SortingCriteria sortingCriteria)
        {
            sortingCriteria.Add(new SortBy() { Direction = SortDirection.ASC, Name = "Name" });
        }

        protected override void InitializeAddingItem(UserItemEditorVM editorVM)
        {
            base.InitializeAddingItem(editorVM);
            editorVM.GroupId = SearchCriteria.ParentId;
        }

        public override void ClearCriteria()
        {
            var tmpParentId = SearchCriteria.ParentId;
            base.ClearCriteria();
            SearchCriteria.ParentId = tmpParentId;
        }

    }
}
