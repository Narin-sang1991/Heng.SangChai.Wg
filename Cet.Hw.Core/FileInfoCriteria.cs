using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Hw.Core
{
    public class FileInfoCriteria
    {
        public Guid? Id { get; set; }
        public Guid? RelationId { get; set; }
        public FileInfoType? RelationType { get; set; }
        public FileInfoType? StartRelationType { get; set; }
        public FileInfoType? EndRelationType { get; set; }
        public string FileName { get; set; }
    }
}
