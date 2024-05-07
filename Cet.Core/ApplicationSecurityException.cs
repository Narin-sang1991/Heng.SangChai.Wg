using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Cet.Core
{
    [DataContract]
    public class ApplicationSecurityException
    {
        [DataMember]
        public string Message { get; set; }
    }
}
