using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Microsoft.Practices.Unity;

namespace Cet.SmartClient.Client
{
    public interface ITreeNodeEditorVM
    {
        void PrepareSubTree();
        event SubNodeInitializingEventHandler SubNodeInitializing;
    }

    public abstract class TreeNodeEditorVMBase<T> : EditorVMBase<T>, ITreeNodeEditorVM where T : new()
    {
        public TreeNodeEditorVMBase() : base() { }
        public TreeNodeEditorVMBase(IUnityContainer container)
            : base(container)
        {
            IsLoadOnDemandEnable = true;
        }

        public ListCollectionView SubTreeNodes
        {
            get { return GetChildNode("SubTreeNodes"); }
        }

        private bool isLoadOnDemandEnable;
        public bool IsLoadOnDemandEnable
        {
            get { return isLoadOnDemandEnable; }
            set
            {
                isLoadOnDemandEnable = value;
                OnPropertyChanged("IsLoadOnDemandEnable");
            }
        }

        public abstract void PrepareSubTree();
        public void ClearSubTree()
        {
            ClearChildNode("SubTreeNodes");
        }

        public void AddSubTreeNode(object node)
        {
            //var extensions = Extensions.FindAll<ISubTreeNodeInitializer>();
            //foreach (ISubTreeNodeInitializer extension in extensions)
            //{
            //    extension.Initialize(node);
            //}
            if (SubNodeInitializing != null)
                SubNodeInitializing(this, new SubNodeInitializingEventArgs() { Node = node });
            AddChildNode(node, "SubTreeNodes");
        }

        public void RemoveSubTreeNode(object node)
        {
            RemoveChildNode(node, "SubTreeNodes");
        }

        #region ITreeNodeEditorVM Members


        public event SubNodeInitializingEventHandler SubNodeInitializing;

        #endregion
    }
}
