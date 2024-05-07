using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Hw.Core
{
    public class OrganizationUnitCriteria
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public bool? IsRootNode { get; set; }
        public bool? IsActive { get; set; }
    }
}
