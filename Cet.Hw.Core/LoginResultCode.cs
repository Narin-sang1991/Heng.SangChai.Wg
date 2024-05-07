using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Hw.Core
{
    public enum LoginResultCode
    {
        Success = 1,
        Invalid = 2,
        PasswordExpire = 3,
        RequiredOU = 4,
        NotActivated = 5
    };

    public class LoginResult
    {
        public LoginResultCode LoginResultCode { get; set; }
        public string Token { get; set; }
    }
}
