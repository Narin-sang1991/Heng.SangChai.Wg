using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Cet.SmartClient.Client
{
    public class UnityViewModel : ViewModelBase
    {
        public UnityViewModel()
        {
        }

        public UnityViewModel(IUnityContainer container)
        {
            if (container != null)
                this.container = container;
        }

        private IUnityContainer container;
        public IUnityContainer Container
        {
            get { return container; }
            set
            {
                this.container = value;
            }
        }
    }
}