using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Cet.SmartClient.Client
{
    public class ChannelFactoryProxy<T>
    {
        string configName;
        ChannelFactory<T> channelFactory = null;

        public ChannelFactoryProxy(string configName)
        {
            this.configName = configName;
        }

        public T CreateChannel()
        {
            if (channelFactory == null) channelFactory = new ChannelFactory<T>(configName);
            return channelFactory.CreateChannel();
        }
    }
}
