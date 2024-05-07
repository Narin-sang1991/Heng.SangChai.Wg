using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Hw.Core
{
    public class BaseStdCriteria
    {
        public Guid? Id { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class BaseStdItemCriteria
    {
        public Guid? Id { get; set; }
        public Guid? ParentId { get; set; }
    }

}
