using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Core
{
    public static class MD5Extension
    {
        public static string ComputeHash(this System.Security.Cryptography.MD5 md5, string value)
        {
            byte[] hashValue = md5.ComputeHash(new UTF8Encoding().GetBytes(value));
            return BitConverter.ToString(hashValue).Replace("-", "").ToLowerInvariant();
        }
    }
}
