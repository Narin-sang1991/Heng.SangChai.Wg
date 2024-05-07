using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using System.ComponentModel;
using Cet.SmartClient.Client;

namespace Cet.SmartClient.Client
{
    public class WinTouchRootDocEditorVM<T> : RootDocEditorVMBase<T>, IOpenEditorVM<Guid>
        where T : new()
    {
        public WinTouchRootDocEditorVM()
            : this(null)
        { }

        public WinTouchRootDocEditorVM(IUnityContainer container)
            : base(container)
        { }


        private EditorVMBase parentVM;
        public EditorVMBase ParentVM
        {
            get { return parentVM; }
            set
            {
                parentVM = value;
                OnPropertyChanged("ParentVM");
            }
        }

    }
}
