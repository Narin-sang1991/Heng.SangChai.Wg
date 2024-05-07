using System.Runtime.Serialization;

namespace Cet.Core
{
    [DataContract]
    public class ResponeWrongful
    {

        [DataMember]
        public string Message { get; set; }
    }
}
