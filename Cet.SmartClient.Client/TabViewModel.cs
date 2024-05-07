using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Windows;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace Cet.SmartClient.Client
{
    public class TabViewModel : UnityViewModel, IWeakEventListener
    {
        public TabViewModel()
            : this(null)
        {
        }
        public TabViewModel(IUnityContainer container)
            : base(container)
        {
            WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.AddEventListener(this);
            NeedMainScrollViewer = true;

            ChildNodeGroups = new Dictionary<string, ListCollectionView>();
            GetChildNode();
            OnPropertyChanged("ChildNodes");
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        private bool needMainScrollViewer;
        public bool NeedMainScrollViewer
        {
            get { return needMainScrollViewer; }
            set
            {
                needMainScrollViewer = value;
                OnPropertyChanged("NeedMainScrollViewer");
            }
        }

        private object header;
        public object Header
        {
            get { return header; }
            set
            {
                header = value;
                OnPropertyChanged("Header");
            }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        private bool isExpanded;
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }
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
            OnPropertyChanged("Header");
        }

        public ListCollectionView ChildNodes
        {
            get { return GetChildNode(); }
        }

        public Dictionary<string, ListCollectionView> ChildNodeGroups { get; private set; }

        public ListCollectionView GetChildNode(string groupName = "default")
        {
            if (!ChildNodeGroups.ContainsKey(groupName))
            {
                ChildNodeGroups.Add(groupName, new ListCollectionView(new ObservableCollection<object>()));
                ChildNodeGroups[groupName].CurrentChanged += new EventHandler(OnCurrentChanged);
            }

            return ChildNodeGroups[groupName];
        }

        public void ClearChildNode(string groupName = "default")
        {
            if (!ChildNodeGroups.ContainsKey(groupName)) return;

            ((ObservableCollection<object>)ChildNodeGroups[groupName].SourceCollection).Clear();
        }

        protected virtual void OnCurrentChanged(object sender, EventArgs e) { }

        public virtual void RemoveChildNode(object node, string groupName = "default")
        {
            ((ObservableCollection<object>)ChildNodeGroups[groupName].SourceCollection).Remove(node);
        }

        public virtual void AddChildNode(object node, string groupName = "default")
        {
            if (!ChildNodeGroups.ContainsKey(groupName))
            {
                ChildNodeGroups.Add(groupName, new ListCollectionView(new ObservableCollection<object>()));
                ChildNodeGroups[groupName].CurrentChanged += new EventHandler(OnCurrentChanged);
            }

            ((ObservableCollection<object>)ChildNodeGroups[groupName].SourceCollection).Add(node);
        }

        public virtual void Dispose() { }
    }
}
