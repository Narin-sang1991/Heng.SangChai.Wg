using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;

namespace Cet.SmartClient.Client
{
    public class MessageHeaderInserterBehavior : IEndpointBehavior
    {
        IUnityContainer container;

        public MessageHeaderInserterBehavior(IUnityContainer container)
        {
            this.container = container;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            MessageHeaderInserter inserter = container.Resolve<MessageHeaderInserter>();
            clientRuntime.MessageInspectors.Add(inserter);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            throw new Exception("Behavior not supported on the consumer side!");
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }

    public class MessageHeaderInserter : IClientMessageInspector
    {
        IUnityContainer container;

        public MessageHeaderInserter(IUnityContainer container)
        {
            this.container = container;
        }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            IEnumerable<MessageHeaderInserterBase> inserters = container.ResolveAll<MessageHeaderInserterBase>();
            foreach (MessageHeaderInserterBase inserter in inserters)
            {
                System.ServiceModel.MessageHeader<string> mhg = new System.ServiceModel.MessageHeader<string>(inserter.GetHeaderData());
                System.ServiceModel.Channels.MessageHeader untyped = mhg.GetUntypedHeader(inserter.Name, inserter.NameSpace);
                request.Headers.Add(untyped);
            }
            return null;
        }
    }

}
