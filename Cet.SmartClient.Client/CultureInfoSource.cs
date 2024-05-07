using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;

namespace Cet.SmartClient.Client
{
    public class CultureInfoSource
    {
        public CultureInfo Current
        {
            get { return Thread.CurrentThread.CurrentCulture; }
        }
    }
}
