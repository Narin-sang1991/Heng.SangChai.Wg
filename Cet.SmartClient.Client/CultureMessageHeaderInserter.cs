using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Cet.SmartClient.Client
{
    public class CultureMessageHeaderInserter : MessageHeaderInserterBase
    {
        public CultureMessageHeaderInserter()
            : base("ns", "culture")
        {
        }

        public override string GetHeaderData()
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
        }
    }
}
