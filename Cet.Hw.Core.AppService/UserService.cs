using Cet.Core;
using Cet.Core.Data;
using Cet.Hw.Core.Domain;
using Cet.Hw.Core;
using Cet.EntityFramework.Adaptor;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Cet.Hw.Core.AppServiceContract;

namespace Cet.Hw.Core.AppService
{
    public class UserService : IUserService
    {
        private IUnityContainer Container { get; set; }

        public IUserIntroductory UserIntroductory { get; set; }

        public UserService(IUnityContainer container)
        {
            Container = container;
            UserIntroductory = Container.Resolve<IUserIntroductory>();
        }

        public Guid Save(UserData data)
        {
            //Check User Profile.
            var permissionService = Container.Resolve<IDomainAuthenticationService>(AuthType.MyAccount.ToString());
            var userProfile = permissionService.GetCurrentUserProfile();
            if (permissionService.PermissionAllow(userProfile.UserId, Container.Resolve<Guid>("AdminGroupId")))
            {
                var service = Container.Resolve<UserServiceBase>();
                var user = service.Save(data);
                UserIntroductory.UnitOfWork.Commit();
                return user.Id;
            }
            else
                throw new FaultException<UserNotLoginException>(new UserNotLoginException() { Message = "Access Denied !" });
        }

        public void Remove(Guid userId)
        {
            //Check User Profile.
            var permissionService = Container.Resolve<IDomainAuthenticationService>(AuthType.MyAccount.ToString());
            var userProfile = permissionService.GetCurrentUserProfile();
            if (permissionService.PermissionAllow(userProfile.UserId, Container.Resolve<Guid>("AdminGroupId")))
            {
                var service = Container.Resolve<UserServiceBase>();
                service.Remove(userId);
                UserIntroductory.UnitOfWork.Commit();
            }
            else
                throw new FaultException<UserNotLoginException>(new UserNotLoginException() { Message = "Access Denied !" });
        }

        public IList<UserData> Find(UserCriteria01 criteria, SortingCriteria sortingCriteria, PagingCriteria pagingCriteria)
        {
            var permissionService = Container.Resolve<IDomainAuthenticationService>(AuthType.MyAccount.ToString());
            var userProfile = permissionService.GetCurrentUserProfile();
            if (permissionService.PermissionAllow(userProfile.UserId, Container.Resolve<Guid>("AdminGroupId")))
            {
                var service = Container.Resolve<UserServiceBase>();
                return service.FindUserInternal(criteria, sortingCriteria, pagingCriteria);
            }
            else
                throw new FaultException<UserNotLoginException>(new UserNotLoginException() { Message = "Access Denied !" });
        }

        public int Count(UserCriteria01 criteria)
        {
            var permissionService = Container.Resolve<IDomainAuthenticationService>(AuthType.MyAccount.ToString());
            var userProfile = permissionService.GetCurrentUserProfile();
            if (permissionService.PermissionAllow(userProfile.UserId, Container.Resolve<Guid>("AdminGroupId")))
            {
                var service = Container.Resolve<UserServiceBase>();
                return service.CountUserInternal(criteria);
            }
            else
                throw new FaultException<UserNotLoginException>(new UserNotLoginException() { Message = "Access Denied !" });
        }

        public IList<BaseStdData> FindUserFromParent(Guid parentId)
        {
            var permissionService = Container.Resolve<IDomainAuthenticationService>(AuthType.MyAccount.ToString());
            var userProfile = permissionService.GetCurrentUserProfile();
            if (permissionService.PermissionAllow(userProfile.UserId, Container.Resolve<Guid>("AdminGroupId")))
            {
                var service = Container.Resolve<UserServiceBase>();
                return service.FindUserFromParent(parentId);
            }
            else
                throw new FaultException<UserNotLoginException>(new UserNotLoginException() { Message = "Access Denied !" });
        }

        public IList<BaseStdData> FindParent(Guid parentId)
        {
            var permissionService = Container.Resolve<IDomainAuthenticationService>(AuthType.MyAccount.ToString());
            var userProfile = permissionService.GetCurrentUserProfile();
            if (permissionService.PermissionAllow(userProfile.UserId, Container.Resolve<Guid>("AdminGroupId")))
            {
                var service = Container.Resolve<UserServiceBase>();
                return service.FindParent(parentId);
            }
            else
                throw new FaultException<UserNotLoginException>(new UserNotLoginException() { Message = "Access Denied !" });
        }


    }
}
