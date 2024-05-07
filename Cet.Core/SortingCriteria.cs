using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Core.Data
{
    public class SortingCriteria : List<SortBy>
    {
    }

    public enum SortDirection { ASC = 0, DESC = 1 };

    public class SortBy
    {
        public string Name { get; set; }
        public SortDirection Direction { get; set; }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(this, obj)) return true;
            SortBy criteria = obj as SortBy;
            if (criteria == null) return false;
            return (this.Name == criteria.Name) && (this.Direction == criteria.Direction);
        }
    }

}
