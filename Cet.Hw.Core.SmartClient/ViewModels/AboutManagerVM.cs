using Cet.SmartClient.Client;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Hw.Core.SmartClient.ViewModels
{
    public class AboutManagerVM : ViewModelBase
    {
        private IUnityContainer container;

        public object Header { get; set; }

        public AboutManagerVM(IUnityContainer container)
        {
            this.container = container;
            GetVersion();
        }

        private string version;
        public string Version
        {
            get { return version; }
            set
            {
                version = value;
                OnPropertyChanged("Version");
            }
        }

        public void GetVersion()
        {
            string versions = "";
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                versions = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            this.Version = versions;
        }

    }
}
