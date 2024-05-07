using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Hw.Core
{
    public class FileInfoUploadData
    {

        public Guid? Id { get; set; }
        public string FileName { get; set; }
        public Guid? RelationId { get; set; }
        public FileInfoType RelationType { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public byte[] FileByteData { get; set; }
        public string FilePathURL { get; set; }
    }
}

