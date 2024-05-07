using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Cet.SmartClient.Client
{
    public class SecurityTokenMessageHeaderInserter : MessageHeaderInserterBase
    {
        IUnityContainer container; 

        public SecurityTokenMessageHeaderInserter(IUnityContainer container) : base("ns", "token")
        {
            this.container = container;
        }

        public override string GetHeaderData()
        {
            UserProfileToken token = container.Resolve<UserProfileToken>();
            if (token == null) return string.Empty;
            return token.Token;
        }
    }
}
