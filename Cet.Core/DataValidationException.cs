using System.Runtime.Serialization;

namespace Cet.Core
{

    [DataContract]
    public class DataValidationException
    {
        [DataMember]
        public string Message { get; set; }
    }

}
