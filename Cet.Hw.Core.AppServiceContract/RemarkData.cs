using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Hw.Core.AppServiceContract
{
    public class RemarkData
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public string Value { get; set; }
    }
}
