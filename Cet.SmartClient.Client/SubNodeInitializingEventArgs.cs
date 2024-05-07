using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cet.SmartClient.Client
{
    public class SubNodeInitializingEventArgs : EventArgs
    {
        public object Node;
    }

    public delegate void SubNodeInitializingEventHandler(object sender, SubNodeInitializingEventArgs e);

}
