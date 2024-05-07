using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Cet.SmartClient.Client
{
    public class TreeSearchVMBase : EditableContainerBase
    {
        public TreeSearchVMBase(IUnityContainer container)
            : base(container)
        {
            this.RootNodes = new ObservableCollection<object>();
        }

        private object selectedItem;
        public object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (!this.HasEditingChilds)
                {
                    selectedItem = value;
                    OnPropertyChanged("SelectedItem");

                    OnCurrentChanged();
                }
            }
        }


        private ObservableCollection<object> rootNodes;
        public ObservableCollection<object> RootNodes
        {
            get { return rootNodes; }
            set
            {
                rootNodes = value;
                OnPropertyChanged("RootNodes");
            }
        }

        public virtual void PrepareRootNodes()
        {
        }

        protected void OnCurrentChanged()
        {
            ClearChildNode();
            if (this.SelectedItem == null) return;

            AddChildNode(this.SelectedItem);

            EditableContainerBase editableContainer = this.SelectedItem as EditableContainerBase;
            if (editableContainer != null)
                editableContainer.Activate();
        }
    }
}
