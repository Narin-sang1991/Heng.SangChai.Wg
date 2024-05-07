using System.Runtime.Serialization;

namespace Cet.Core
{
    [DataContract]
    public class UserNotLoginException
    {
        [DataMember]
        public string Message { get; set; }
    }
}
