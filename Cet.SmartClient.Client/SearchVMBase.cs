using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cet.Core.Data;
using Microsoft.Practices.Unity;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace Cet.SmartClient.Client
{
    /// <summary>
    /// Container SearchResult of the specified type.
    /// Search implementation which will call SearchItems and CountItems.
    /// </summary>
    /// <typeparam name="ItemType"></typeparam>
    /// <typeparam name="OriginalSourceType"></typeparam>
    /// <typeparam name="SearchCriteriaType"></typeparam>
    public abstract class SearchOnlyVMBase<ItemType, OriginalSourceType, SearchCriteriaType> : SearchVMBase
        where OriginalSourceType : new()
        where SearchCriteriaType : new()
    {
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

        public SearchOnlyVMBase(IUnityContainer container)
            : base(container)
        {
            this.SearchCriteria = new SearchCriteriaType();
            this.SearchResult = new ObservableCollection<ItemType>();
            this.SearchResultCollectionView = container.Resolve<FsListCollectionViewBase>(new ParameterOverride("list", this.SearchResult));
            this.SearchResultCollectionView.PageChanging += new FsListCollectionViewBase.PageChangingHandler(OnPageChanging);
        }

        SearchCriteriaType searchCriteria;
        public SearchCriteriaType SearchCriteria
        {
            get { return searchCriteria; }
            set { searchCriteria = value; OnPropertyChanged("SearchCriteria"); }
        }

        protected virtual int CountItemsInternal(SearchCriteriaType searchCriteria)
        {
            throw new NotImplementedException();
        }

        protected virtual IList<OriginalSourceType> SearchInternal(SearchCriteriaType searchCriteria, SortingCriteria sortingCriteria, PagingCriteria pagingCriteria)
        {
            throw new NotImplementedException();
        }

        private ICommand searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (searchCommand == null)
                    searchCommand = new DelegateCommand(Search, Search_CanExecute);

                return searchCommand;
            }
        }

        private ICommand searchAsyncCommand;
        public ICommand SearchAsyncCommand
        {
            get
            {
                if (searchAsyncCommand == null)
                    searchAsyncCommand = new DelegateCommand(async () => { await SearchAsync(); }, Search_CanExecute);

                return searchAsyncCommand;
            }
        }

        private ICommand clearCriteriaCommand;
        public ICommand ClearCriteriaCommand
        {
            get
            {
                if (clearCriteriaCommand == null)
                    clearCriteriaCommand = new DelegateCommand(ClearCriteria);

                return clearCriteriaCommand;
            }
        }

        public override void PrepareChildVMs()
        {
            SearchAsync();
        }

        public override void Search()
        {
            DispatcherFrame frame = new DispatcherFrame();
            RunWorkerCompletedEventArgs e = null;
            System.Globalization.CultureInfo uiOldCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            var bw = new BackgroundWorker();
            bw.DoWork += (sender, arg) =>
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = uiOldCulture;
                PagingCriteria pagingCriteria = null;
                if (EnablePaging && PageSize > 0)
                {
                    ItemCount = CountItemsInternal(SearchCriteria);
                    pagingCriteria = new PagingCriteria() { PageIndex = this.PageIndex, PageSize = this.PageSize };
                }

                if (SortingCriteria.Count == 0) PrepareDefaultSortingCriteria(SortingCriteria);

                IList<OriginalSourceType> items = SearchInternal(SearchCriteria, SortingCriteria, pagingCriteria);
                arg.Result = items;
            };

            bw.RunWorkerCompleted += (sender, arg) =>
            {
                frame.Continue = false;
                IsBusy = false;
                e = arg;
                if (e.Error != null) return;
                PrepareListViewCollection(arg.Result as IList<OriginalSourceType>);
            };

            IsBusy = true;
            bw.RunWorkerAsync();
            Dispatcher.PushFrame(frame);
            if (e != null && e.Error != null) throw e.Error;
        }

        public override async Task SearchAsync()
        {
            IsBusy = true;

            System.Globalization.CultureInfo uiOldCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;

            PagingCriteria pagingCriteria = null;
            try
            {
                if (EnablePaging && PageSize > 0)
                {
                    ItemCount = await Task.Run(() =>
                    {
                        System.Threading.Thread.CurrentThread.CurrentUICulture = uiOldCulture;
                        return CountItemsInternal(SearchCriteria);
                    });
                    pagingCriteria = new PagingCriteria() { PageIndex = this.PageIndex, PageSize = this.PageSize };
                }

                if (SortingCriteria.Count == 0) PrepareDefaultSortingCriteria(SortingCriteria);

                IList<OriginalSourceType> items = await Task.Run(() =>
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = uiOldCulture;
                    return SearchInternal(SearchCriteria, SortingCriteria, pagingCriteria);
                });

                PrepareListViewCollection(items);
            }
            finally
            {
                IsBusy = false;
            }

        }

        protected bool Search_CanExecute()
        {
            return !HasEditingChilds;
        }

        protected virtual void PrepareDefaultSortingCriteria(SortingCriteria sortingCriteria)
        {
            throw new NotImplementedException();
        }

        void OnPageChanging()
        {
            Search();
        }

        protected virtual void PrepareListViewCollection(IList<OriginalSourceType> list)
        {
            SearchResult.Clear();

            object o;

            foreach (OriginalSourceType item in list)
            {
                o = item;
                SearchResult.Add((ItemType)o);
                //SearchResult.Add((ItemType)Convert.ChangeType(item, typeof(ItemType)));
            }
        }

        public virtual void ClearCriteria()
        {
            SearchCriteria = new SearchCriteriaType();
        }

        public ItemType CurrentItem
        {
            get
            {
                return (ItemType)this.SearchResultCollectionView.CurrentItem;
            }
        }

    }

    /// <summary>
    /// Intend for inline grid editing
    /// So, need to hook when item is added to the searh result.
    /// Not allow to change active item if it is under editing. 
    /// </summary>
    /// <typeparam name="EditorVMType"></typeparam>
    /// <typeparam name="OriginalSourceType"></typeparam>
    /// <typeparam name="SearchCriteriaType"></typeparam>
    public class SearchVMBase<EditorVMType, OriginalSourceType, SearchCriteriaType> : SearchOpenVMBase<EditorVMType, OriginalSourceType, SearchCriteriaType>
        where EditorVMType : EditorVMBase<OriginalSourceType>
        where OriginalSourceType : new()
        where SearchCriteriaType : new()
    {

        public SearchVMBase(IUnityContainer container)
            : base(container)
        {
            this.SearchResultCollectionView.CurrentChanging += new CurrentChangingEventHandler(OnCurrentChanging);
            this.SearchResultCollectionView.CurrentChanged += new EventHandler(OnCurrentChanged);
        }

        protected override void OnCurrentChanged(object sender, EventArgs e)
        {
            ClearChildNode();
            if (this.SearchResultCollectionView.CurrentItem == null) return;

            AddChildNode(this.SearchResultCollectionView.CurrentItem);

            EditableContainerBase editableContainer = this.SearchResultCollectionView.CurrentItem as EditableContainerBase;
            if (editableContainer != null)
                editableContainer.Activate();
        }

        void OnCurrentChanging(object sender, CurrentChangingEventArgs e)
        {
            if (HasEditingChilds && e.IsCancelable)
                e.Cancel = true;
            else if (this.SearchResultCollectionView.IsAddingNew)
            {
                InitializeAddingItem(this.SearchResultCollectionView.CurrentAddItem as EditorVMType);
            }
        }

        protected virtual void InitializeAddingItem(EditorVMType editorVM)
        {

            editorVM.PropertyChanged += item_BusyPropertyChanged;

        }
    }


    /// <summary>
    /// Intend to host EditorVM allow removing but not add/update. 
    /// No call to prepare child when changing the active item. 
    /// Support removing by call to EditorVM.remove 
    /// </summary>
    /// <typeparam name="EditorVMType"></typeparam>
    /// <typeparam name="OriginalSourceType"></typeparam>
    /// <typeparam name="SearchCriteriaType"></typeparam>
    public class SearchOpenVMBase<EditorVMType, OriginalSourceType, SearchCriteriaType> : SearchOnlyVMBase<EditorVMType, OriginalSourceType, SearchCriteriaType>
        where EditorVMType : EditorVMBase<OriginalSourceType>
        where OriginalSourceType : new()
        where SearchCriteriaType : new()
    {

        public SearchOpenVMBase(IUnityContainer container)
            : base(container)
        {
        }

        protected override void PrepareListViewCollection(IList<OriginalSourceType> list)
        {

            SearchResult.Clear();

            foreach (OriginalSourceType item in list)
            {
                EditorVMType editorVM = Container.Resolve<EditorVMType>();
                editorVM.OriginalSource = item;
                InitializeLoadingItem(editorVM);
                SearchResult.Add(editorVM);
            }


        }

        protected virtual void item_BusyPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsBusy")
                IsBusy = (sender as EditorVMBase).IsBusy;
        }

        protected virtual void InitializeLoadingItem(EditorVMType editorVM)
        {

            editorVM.PropertyChanged += item_BusyPropertyChanged;

        }
        public override void RemoveItem(object item)
        {
            if (!SearchResultCollectionView.CanRemove) return;

            EditorVMType editor = item as EditorVMType;
            editor.Remove();
            SearchResultCollectionView.Remove(item);
        }

        public override async Task RemoveItemAsync(object item)
        {
            if (!SearchResultCollectionView.CanRemove) return;

            EditorVMType editor = item as EditorVMType;
            await editor.RemoveAsync();
            SearchResultCollectionView.Remove(item);
        }
    }

    /// <summary>
    /// Contain properties for pagining and criteria and Search method
    /// </summary>
    public abstract class SearchVMBase : EditableContainerBase
    {
        public SearchVMBase(IUnityContainer container)
            : base(container)
        {
            this.SortingCriteria = new SortingCriteria();
            EnablePaging = true;
        }

        public int ItemCount
        {
            get { return SearchResultCollectionView.ItemCount; }
            set
            {
                SearchResultCollectionView.ItemCount = value;
                OnPropertyChanged("ItemCount");
            }
        }

        public int PageSize
        {
            get { return SearchResultCollectionView.PageSize; }
            set
            {
                SearchResultCollectionView.PageSize = value;
                OnPropertyChanged("PageSize");
            }
        }

        public int PageIndex
        {
            get { return SearchResultCollectionView.PageIndex; }
            set
            {
                SearchResultCollectionView.PageIndex = value;
                OnPropertyChanged("PageIndex");
            }
        }

        private SortingCriteria sortingCriteria;
        public SortingCriteria SortingCriteria
        {
            get { return sortingCriteria; }
            set
            {
                sortingCriteria = value;
            }
        }

        private bool hasSelectedItem;
        public bool HasSelectedItem
        {
            get { return hasSelectedItem; }
            set
            {
                hasSelectedItem = value;
                OnPropertyChanged("HasSelectedItem");
            }
        }

        private FsListCollectionViewBase searchResultCollectionView;
        public FsListCollectionViewBase SearchResultCollectionView
        {
            get { return searchResultCollectionView; }
            set
            {
                searchResultCollectionView = value;
                OnPropertyChanged("SearchResultCollectionView");
            }
        }

        public bool EnablePaging { get; set; }

        public abstract void Search();

        public virtual Task SearchAsync()
        {
            throw new NotImplementedException();
        }

        //todo: youyz: findout what is this method for
        public virtual void AddItems(ObservableCollection<object> items)
        {
            throw new NotImplementedException();
        }

        //todo: youyz: findout what is this method for
        public virtual void AddItem(EditorVMBase item)
        {
            throw new NotImplementedException();
        }

        public virtual void RemoveItem(object item)
        {
            throw new NotImplementedException();
        }

        public virtual Task RemoveItemAsync(object item)
        {
            throw new NotImplementedException();
        }
    }
}
