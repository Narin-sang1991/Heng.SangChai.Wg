using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Core
{
    public class ChannelInjectionFactory<T>
    {
        public static T CreateChannel(IUnityContainer c)
        {
            ChannelFactory<T> channelFactory = c.Resolve<ChannelFactory<T>>();
            EndpointAddress oldAddress = channelFactory.Endpoint.Address;
            if (channelFactory.Endpoint.Address.ToString().StartsWith(@"http://dummy"))
            {
                string baseUrl = c.Resolve<string>("ServerBaseUrl");
                channelFactory.Endpoint.Address = new EndpointAddress(baseUrl + oldAddress.Uri.AbsolutePath);
            }
            return channelFactory.CreateChannel();
        }
    }
}
