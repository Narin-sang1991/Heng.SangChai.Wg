using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cet.SmartClient.Client
{
    public class UserProfileToken
    {
        public string Token { get; private set; }
        public UserProfileToken(string token)
        {
            Token = token;
        }
    }
}
