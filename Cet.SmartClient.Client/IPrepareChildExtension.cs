using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Cet.SmartClient.Client
{
    public interface IPrepareChildExtension : IExtension<EditableContainerBase>
    {
        void PrepareChildVMs(object sender, EventArgs e);
    }
}
