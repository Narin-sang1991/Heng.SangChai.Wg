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
    public class MessageHeaderInserterBehaviorElement : BehaviorExtensionElement
    {
        IUnityContainer container;

        public MessageHeaderInserterBehaviorElement()
        {
            this.container = ServiceLocator.Current.GetInstance<IUnityContainer>();
        }

        public override Type BehaviorType
        {
            get { return typeof(MessageHeaderInserterBehavior); }
        }

        protected override object CreateBehavior()
        {
            return container.Resolve<MessageHeaderInserterBehavior>();
        }
    }

}