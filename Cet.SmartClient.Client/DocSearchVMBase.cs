using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;
using Cet.Core.Data;
using System.ComponentModel;

namespace Cet.SmartClient.Client
{
    public class DocSearchVMBase<ItemType, OriginalSourceType> : DocEditorVMBase<List<OriginalSourceType>>
        where ItemType : DocEditorVMBase<OriginalSourceType>
        where OriginalSourceType : new()
    
    {
        int pageSize;
        public int PageSize
        {
            get { return pageSize; }
            set
            {
                pageSize = value;
                OnPropertyChanged("PageSize");
            }
        }

        private UnityEditableCollectionView searchResultCollectionView;
        public UnityEditableCollectionView SearchResultCollectionView
        {
            get { return searchResultCollectionView; }
            set
            {
                searchResultCollectionView = value;
                OnPropertyChanged("SearchResultCollectionView");
            }
        }

        private ObservableCollection<ItemType> searchResult;
        public ObservableCollection<ItemType> SearchResult
        {
            get { return searchResult; }
            set
            {
                searchResult = value;
                OnPropertyChanged("SearchResult");
            }
        }
        public DocSearchVMBase() { }
        public DocSearchVMBase(IUnityContainer container)
            : base(container)
        {
            this.SearchResult = new ObservableCollection<ItemType>();
            this.SearchResultCollectionView = container.Resolve<UnityEditableCollectionView>(new ParameterOverride("list", this.SearchResult));
            this.SearchResultCollectionView.CurrentChanging += new CurrentChangingEventHandler(OnCurrentChanging);
            this.SearchResultCollectionView.CurrentChanged += new EventHandler(OnCurrentChanged);
            this.SearchResultCollectionView.NotifyRemoveItem += NotifyRemoveItem;
        }

        public override void LoadOriginalSource(List<OriginalSourceType> originalSource)
        {
            LoadOriginalSource(originalSource, null);
        }

        public virtual void LoadOriginalSource(List<OriginalSourceType> originalSource, object rootOriginalSource)
        {
            SearchResult.Clear();
            ClearChildNode();
            foreach (OriginalSourceType item in originalSource)
            {
                ItemType editorVM = Container.Resolve<ItemType>();
                PreInitializeLoadingItem(editorVM);
                editorVM.LoadOriginalSource(item);
                InitializeLoadingItem(editorVM);
                SearchResult.Add(editorVM);
            }
        }

        public virtual void NotifyRemoveItem()
        { 
        
        }

        protected virtual void PreInitializeLoadingItem(ItemType editorVM)
        {

        }

        public virtual void SetSortingCriteria(string propertyName, ListSortDirection sortingDirection)
        {
            SearchResultCollectionView.SortDescriptions.Add(new SortDescription(propertyName, sortingDirection));
        }

        public override void SaveOriginalSource(List<OriginalSourceType> originalSource)
        {
            SaveOriginalSource(originalSource, null);
        }

        public virtual void SaveOriginalSource(List<OriginalSourceType> originalSource, object rootOriginalSource)
        {
            foreach (ItemType item in SearchResult)
            {
                var o = Container.Resolve<OriginalSourceType>();
                item.SaveOriginalSource(o);
                originalSource.Add(o);
            }
        }

        protected virtual void InitializeLoadingItem(ItemType editorVM) { }

        protected override void OnCurrentChanged(object sender, EventArgs e)
        {
            ClearChildNode();
            if (this.SearchResultCollectionView.CurrentItem == null) return;

            AddChildNode(this.SearchResultCollectionView.CurrentItem);
        }

        void OnCurrentChanging(object sender, CurrentChangingEventArgs e)
        {
            if (this.SearchResultCollectionView.IsAddingNew)
            {
                InitializeAddingItem(this.SearchResultCollectionView.CurrentAddItem as ItemType);
            }
        }

        protected virtual void InitializeAddingItem(ItemType editorVM)
        {
            editorVM.BeginEdit();
        }

        public override void BeginEdit()
        {
            this.IsEditing = true;
            foreach (ItemType item in SearchResult)
            {
                item.BeginEdit();
            }
        }

        public virtual void AddItem(ItemType editorVM)
        {
            this.SearchResultCollectionView.AddNewItem(editorVM);
            this.SearchResultCollectionView.CommitNew();
        }

    }
}
