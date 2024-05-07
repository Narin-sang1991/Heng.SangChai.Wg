using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Practices.Unity;
using System.Windows;
using System.Windows.Data;
using System.ServiceModel;

namespace Cet.SmartClient.Client
{
    public class EditableContainerBase : TabViewModel, IEditableObjectContainer, IExtensibleObject<EditableContainerBase>
    {
        public EditableContainerBase()
            : this(null)
        {
        }

        public EditableContainerBase(IUnityContainer container)
            : base(container)
        {


            extensions = new ExtensionCollection<EditableContainerBase>(this);
        }

        public EditableContainerBase Parent { get; protected set; }

        private bool isFullLoad;

        public delegate bool CheckClosingWindowDelegate();
        public event CheckClosingWindowDelegate CheckClosingWindowEvent;

        public bool CheckClosingEvent()
        {
            bool result = false;
            if (CheckClosingWindowEvent != null)
                result = CheckClosingWindowEvent();

            return result;
        }

        private bool isEditing;
        public bool IsEditing
        {
            get { return isEditing; }
            protected set
            {
                isEditing = value;
                CalculateHasEditingChilds();
                OnPropertyChanged("IsEditing");
                OnPropertyChanged("IsNotEditing");
            }
        }

        public bool IsNotEditing
        {
            get { return !isEditing; }
        }

        private bool hasEditingChilds;
        /// <summary>
        /// HasEditingChilds is true when this VM is editing or has child in editing
        /// </summary>
        public bool HasEditingChilds
        {
            get { return hasEditingChilds; }
            protected set
            {
                hasEditingChilds = value;
                OnPropertyChanged("HasEditingChilds");
            }
        }

        bool allowDelete = true;
        public bool AllowDelete
        {
            get { return allowDelete; }
            set
            {
                allowDelete = value;
                CanDeleteItem = allowDelete;
                OnPropertyChanged("AllowDelete");
            }
        }

        private bool canDeleteItem;
        public bool CanDeleteItem
        {
            get { return canDeleteItem; }
            protected set
            {
                canDeleteItem = value;
                OnPropertyChanged("CanDeleteItem");
            }
        }


        bool allowEdit = true;
        public bool AllowEdit
        {
            get { return allowEdit; }
            set
            {
                allowEdit = value;
                OnPropertyChanged("AllowEdit");
                CalculateCanEdit();
            }
        }

        bool canEdit = true;
        public virtual bool CanEdit
        {
            get
            {
                return canEdit;
            }
            protected set
            {
                canEdit = value;
                OnPropertyChanged("CanEdit");
                OnPropertyChanged("IsReadOnly");
            }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                return !canEdit;
            }
        }

        protected virtual void CalculateCanEdit()
        {
            if (!AllowEdit) CanEdit = false;
            else if (Parent != null) CanEdit = Parent.CanEdit;
            else CanEdit = AllowEdit;
        }

        protected void CalculateHasEditingChilds()
        {
            if (IsEditing)
            {
                HasEditingChilds = true;
                return;
            }

            HasEditingChilds = CheckEditingChilds();
        }

        protected void OnNotifyChange(object sender, PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals("HasEditingChilds") && !e.PropertyName.Equals("CanEdit")) return;

            this.CalculateHasEditingChilds();
            CanDeleteItem = !HasEditingChilds;
        }

        public override void RemoveChildNode(object node, string groupName = "default")
        {
            base.RemoveChildNode(node, groupName);

            if (node is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)node).PropertyChanged -= OnNotifyChange;
            }

            EditableContainerBase child = node as EditableContainerBase;
            if (child != null)
            {
                child.Parent = null;
                this.PropertyChanged -= new PropertyChangedEventHandler(child.OnParentPropertyChanged);
                child.CalculateCanEdit();
            }
        }

        public override void AddChildNode(object node, string groupName = "default")
        {
            base.AddChildNode(node, groupName);

            if (node is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)node).PropertyChanged += OnNotifyChange;
            }

            EditableContainerBase child = node as EditableContainerBase;
            if (child != null)
            {
                child.Parent = this;
                this.PropertyChanged += new PropertyChangedEventHandler(child.OnParentPropertyChanged);
                child.CalculateCanEdit();
            }
        }

        protected void OnParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals("CanEdit")) return;
            CalculateCanEdit();
        }

        protected virtual bool CheckEditingChilds()
        {
            foreach (string key in ChildNodeGroups.Keys)
            {
                foreach (object child in ChildNodeGroups[key])
                {
                    IEditableObjectContainer container = child as IEditableObjectContainer;
                    if (container == null) continue;
                    if (container.HasEditingChilds) return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Create children ViewModel and add to child node.
        /// </summary>
        public virtual void PrepareChildVMs()
        {

        }

        /// <summary>
        /// Called when the ViewModel has been chosen on the screen. If the ViewModel has not yet loaded, the system will load it. 
        /// </summary>
        public virtual void Activate()
        {
            if (!isFullLoad)
            {
                FullLoad();
            }
        }

        protected virtual void FullLoad()
        {
            ClearChildNode();
            PrepareChildVMs();
            if (ChildVMsPreparing != null)
                ChildVMsPreparing(this, EventArgs.Empty);
            isFullLoad = true;
        }

        protected override void OnCurrentChanged(object sender, EventArgs e)
        {
            EditableContainerBase editor = ((ListCollectionView)sender).CurrentItem as EditableContainerBase;
            if (editor == null) return;

            editor.Activate();
        }


        public delegate void ChildVMsPreparingEventHandler(object sender, EventArgs e);
        public event ChildVMsPreparingEventHandler ChildVMsPreparing;

        public void AddExtensions(IExtension<EditableContainerBase>[] extensions)
        {
            foreach (IExtension<EditableContainerBase> extension in extensions)
            {
                this.extensions.Add(extension);
            }
        }

        #region IExtensibleObject<EditableContainerBase> Members

        private ExtensionCollection<EditableContainerBase> extensions;
        public IExtensionCollection<EditableContainerBase> Extensions
        {
            get { return extensions; }
        }

        #endregion
    }
}
