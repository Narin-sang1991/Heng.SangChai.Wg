using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using Cet.Core.Data;
using Cet.Hw.Core.AppServiceContract;
using Cet.EntityFramework.Adaptor;
using Cet.Hw.Core.Domain;

namespace Cet.Hw.Core.AppService
{
    public class MenuService : IMenuService
    {

        IUnityContainer container;

        public MenuService(IUnityContainer container)
        {
            this.container = container;
        }

        public IList<MenuData01> Find(MenuCriteria01 criteria, SortingCriteria sortingCriteria, PagingCriteria pagingCriteria)
        {
            var repository = container.Resolve<IMenuIntroductory>();
            var result = repository.GetAll().Where(MenuSpecification.GetSpecification01(criteria)).ToList().AsQueryable();

            if (sortingCriteria != null) result = result.OrderBy(sortingCriteria);
            if (pagingCriteria != null) result = result.Page(pagingCriteria);
            var result2 = result.SelectMany(t => t.MenuTranslates).Where(t => t.LanguageCode == System.Threading.Thread.CurrentThread.CurrentUICulture.Name).ToList();

            var menuTranslates = result.GroupJoin(result2.AsQueryable(), t1 => t1.Code, t2 => t2.Code, (menu, translates) => new MenuData01()
            {
                Code = menu.Code,
                Command = menu.Command,
                ParentCode = menu.ParentCode,
                ResourceUID = menu.ResourceUID,
                Ordinary = menu.Ordinary,
                Name = translates.Count() == 0 ? menu.Name : translates.FirstOrDefault().Name
            });
            return menuTranslates.ToList();
        }
        Func<Menu, MenuData01> menuToMenudata01 = t => new MenuData01() { Code = t.Code, Name = t.Name, ParentCode = t.ParentCode, ResourceUID = t.ResourceUID, Ordinary = t.Ordinary, Command = t.Command };

        public IList<Guid> GetAccessibleMenuList()
        {
            var repository = container.Resolve<IMenuIntroductory>();
            var result = repository.GetAll().Where(t => t.IsActive == true).Select(t => t.ResourceUID);
            return result.ToList();

        }

        public IList<Guid> GetManageMenuList()
        {
            var introductory = container.Resolve<IMenuIntroductory>();
            var result = introductory.GetAll().Where(t => t.IsActive == true).Select(t => t.ResourceUID);
            return result.ToList();
        }
    }
}
