using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Cet.SmartClient.Client
{
    public interface IEditableObjectContainer
    {
        bool HasEditingChilds { get; }
    }
}