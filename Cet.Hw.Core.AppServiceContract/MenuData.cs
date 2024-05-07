using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Hw.Core.AppServiceContract
{
    public  class MenuData : MenuData01
    {
        [IgnoreDataMember]
        public MenuData ParentMenu { get; set; }
        public List<MenuData> ChildrenMenu { get; set; }
        public bool IsActive { get; set; }
        public MenuData()
        {
            ChildrenMenu = new List<MenuData>();
        }

        public void AddChildMenu(MenuData child)
        {
            child.ParentMenu = this;
            ChildrenMenu.Add(child);
        }

        public void Active()
        {
            this.IsActive = true;
            if (ParentMenu != null)
                ParentMenu.Active();
        }
    }
}
