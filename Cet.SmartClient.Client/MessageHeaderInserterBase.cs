using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;

namespace Cet.SmartClient.Client
{
    public abstract class MessageHeaderInserterBase : IClientMessageInspector
    {
        public MessageHeaderInserterBase(string ns, string name)
        {
            NameSpace = ns;
            Name = name;
        }

        public string NameSpace { get; private set; }
        public string Name { get; private set; }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            System.ServiceModel.MessageHeader<string> mhg = new System.ServiceModel.MessageHeader<string>(GetHeaderData());
            System.ServiceModel.Channels.MessageHeader untyped = mhg.GetUntypedHeader(Name, NameSpace);
            request.Headers.Add(untyped);
            return null;
        }

        public abstract string GetHeaderData();
    }
}
