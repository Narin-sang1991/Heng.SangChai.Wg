using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Cet.Hw.Core
{
    [DataContract]
    [Serializable]
    public class UserProfile
    {
        [DataMember]
        public Guid UserId { get; private set; }

        [DataMember]
        public ReadOnlyCollection<string> GroupCodes { get; private set; }

        [DataMember]
        public Guid? UserUID { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public string Realm { get; private set; }

        public UserProfile(Guid userId, Guid? userUID, string name, string realm)
        {
            this.UserId = userId;
            this.UserUID = userUID;
            this.Name = name;
            this.Realm = realm;
        }

        public void UpdateRealm(string realm)
        {
            this.Realm = realm;
        }
    }
}
