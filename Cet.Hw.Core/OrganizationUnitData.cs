using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Hw.Core
{
    public class OrganizationUnitData
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string TaxID { get; set; }
        public string TelNo { get; set; }
        public string Address { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string ParentCode { get; set; }
        public Guid? ParentId { get; set; }
        public string OuLogoFile { get; set; }
        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                OrganizationUnitData beObj = obj as OrganizationUnitData;
                return this.Id == beObj.Id;
            }
        }

    }
}
