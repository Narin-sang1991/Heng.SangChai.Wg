using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Cet.Hw.Core.AppServiceContract
{
    public class MenuData01
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentCode { get; set; }
        //public string ParentName { get; set; }
        public Guid ResourceUID { get; set; }
        public int Ordinary { get; set; }
        public string Command { get; set; }

    }
}
