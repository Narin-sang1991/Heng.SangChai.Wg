using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;
using System.Windows;
using System.ComponentModel;
using WPFLocalizeExtension.Extensions;

namespace Cet.SmartClient.Client
{
    public class UnityViewBase : UserControl, IWeakEventListener, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private IUnityContainer container;
        public IUnityContainer Container
        {
            get { return container; }
            protected set { container = value; }
        }

        public UnityViewBase()
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                this.container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            }

            WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.AddEventListener(this);
        }

        #region IWeakEventListener Members

        public virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (sender is WPFLocalizeExtension.Engine.LocalizeDictionary)
            {
                OnLanguageChanged();
            }
            return true;
        }

        public virtual void OnLanguageChanged()
        { 
        
        }
        #endregion
    }
}
