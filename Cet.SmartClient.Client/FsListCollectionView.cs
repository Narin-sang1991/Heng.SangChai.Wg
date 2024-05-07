using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Practices.Unity;

namespace Cet.SmartClient.Client
{
    public class UnityEditableCollectionView : ListCollectionView, IEditableCollectionView
    {
        protected IList list;
        protected IUnityContainer container;

        public delegate void RemoveItemHandler();
        public event RemoveItemHandler NotifyRemoveItem;

        public UnityEditableCollectionView(IUnityContainer container, IList list)
            : base(list)
        {
            this.container = container;
            this.list = list;
        }

        public virtual void Clear()
        {
            list.Clear();
        }

        object IEditableCollectionView.AddNew()
        {
            return AddNewItem(container.Resolve(GetItemType(true)));
        }

        void IEditableCollectionView.Remove(object item)
        {
            Remove(item);

            if (NotifyRemoveItem != null)
                NotifyRemoveItem();
        }

        internal Type GetItemType(bool useRepresentativeItem)
        {
            foreach (Type type2 in this.SourceCollection.GetType().GetInterfaces())
            {
                if (type2.Name == typeof(IEnumerable<>).Name)
                {
                    Type[] genericArguments = type2.GetGenericArguments();
                    if (genericArguments.Length == 1)
                    {
                        return genericArguments[0];
                    }
                }
            }
            if (useRepresentativeItem)
            {
                object representativeItem = this.GetRepresentativeItem();
                if (representativeItem != null)
                {
                    return representativeItem.GetType();
                }
            }
            return null;
        }

        internal object GetRepresentativeItem()
        {
            if (!this.IsEmpty)
            {
                IEnumerator enumerator = this.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    if ((current != null) && (current != NewItemPlaceholder))
                    {
                        return current;
                    }
                }
            }
            return null;
        }
    
    }

    public class FsListCollectionViewBase : UnityEditableCollectionView
    {
        public FsListCollectionViewBase(IUnityContainer container, IList list)
            : base(container, list)
        {
        }

        public int PageCount
        {
            get
            {
                int pageCount;
                if (PageSize == 0) pageCount = 1;
                else
                {
                    pageCount = ItemCount / PageSize;
                    if (ItemCount % PageSize != 0)
                        pageCount += 1;
                }
                return pageCount;
            }
        }

        public delegate void PageChangingHandler();
        public event PageChangingHandler PageChanging;

        void OnPageChanging()
        {
            if (PageChanging != null) PageChanging();
        }

        private int itemCount;
        public int ItemCount
        {
            get { return itemCount; }
            set
            {
                itemCount = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("ItemCount"));
            }
        }

        private int pageIndex;
        public int PageIndex
        {
            get { return pageIndex; }
            set
            {
                if (value != pageIndex)
                {
                    pageIndex = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("PageIndex"));
                    OnPageChanging();
                }
            }
        }

        private int pageSize;
        public int PageSize
        {
            get { return pageSize; }
            set
            {
                if (pageSize != value)
                {
                    pageSize = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("PageSize"));
                }
            }
        }

        public bool CanMoveToNext()
        {
            if (PageIndex < PageCount - 1 || CurrentPosition != Count - 1)
                return true;
            return false;
        }

        public bool CanMoveToPrevious()
        {
            if (PageIndex > 0 || CurrentPosition != 0)
                return true;
            return false;
        }

        public override bool MoveCurrentToNext()
        {
            bool canMove = base.MoveCurrentToNext();

            if (canMove) return canMove;

            if (base.IsCurrentAfterLast)
            {
                if (PageIndex < PageCount)
                    PageIndex += 1;

                canMove = base.MoveCurrentToPosition(0);
            }
            return canMove;
        }

        public override bool MoveCurrentToPrevious()
        {
            bool canMove = base.MoveCurrentToPrevious();
            if (canMove) return canMove;

            if (base.IsCurrentBeforeFirst)
            {
                if (PageIndex > 0)
                    PageIndex -= 1;

                canMove = base.MoveCurrentToPosition(Count - 1);
            }

            return canMove;
        }
        public override bool MoveCurrentToFirst()
        {
            if (PageIndex != 0)
            {
                PageIndex = 0;
            }

            return base.MoveCurrentToFirst();
        }

        public override bool MoveCurrentToLast()
        {
            if (PageIndex != PageCount - 1)
            {
                PageIndex = PageCount - 1;
            }

            return base.MoveCurrentToLast();
        }
    }
}
