using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Fs.SmartClient.Client
{
    public interface ISubTreeNodeInitializer : IExtension<EditableContainerBase>
    {
        void Initialize(object node);
    }
}
