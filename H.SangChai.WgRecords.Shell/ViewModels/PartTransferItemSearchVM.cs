using Cet.SmartClient.Client;
using H.SangChai.WgRecords.Shell.Models;
using Microsoft.Practices.Unity;
using System;
using Cet.Core.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Media;
using System.Threading;

namespace H.SangChai.WgRecords.Shell.ViewModels
{
    public class PartTransferItemSearchVM : SearchOnlyVMBase<MeasuringItemData, MeasuringItemData, Object>
    {
        public PartTransferItemSearchVM(IUnityContainer container)
            : base(container)
        {
            PageSize = 10;
            PageIndex = 0;
        }

        protected override IList<MeasuringItemData> SearchInternal(object searchCriteria, SortingCriteria sortingCriteria, PagingCriteria pagingCriteria)
        {
            OnPropertyChanged("SearchResult");
            return SearchResult;
        }

        protected override int CountItemsInternal(object searchCriteria)
        {
            return this.SearchResult.Count();
        }

        internal void SetCurrentClipboard(MeasuringItemData eventSelected)
        {
            var newItems = new List<MeasuringItemData>();
            foreach (var item in this.SearchResult)
            {
                item.CurrentClipboard = (eventSelected.Id == item.Id);
                newItems.Add(item);
            }
            PrepareListViewCollection(newItems);
            CopyWgItemToClipboard(eventSelected.NetWeight);
        }

        internal void CopyWgItemToClipboard(decimal netWeight)
        {
            Clipboard.SetText(netWeight.ToString(), TextDataFormat.Text);
            SystemSounds.Beep.Play();
        }

        protected override void PrepareDefaultSortingCriteria(SortingCriteria sortingCriteria)
        {
            sortingCriteria.Add(new SortBy() { Direction = SortDirection.DESC, Name = "SeqNo" });
        }

        public bool SetWeight(WeightData iWeightData)
        {
            if (iWeightData == null) return false;

            var countAll = this.SearchResult.Count();
            var newId = Guid.NewGuid();
            var data = new MeasuringItemData
            {
                Id = newId,
                SeqNo = countAll + 1,
                TareWeight = iWeightData.TareWeight,
                NetWeight = iWeightData.NetWeight,
                DateTimeStamp = DateTime.Now,
                CurrentClipboard = true,
            };
            var newItems = new List<MeasuringItemData>();
            newItems.Add(data);
            foreach (var item in this.SearchResult)
            {
                item.CurrentClipboard = false;
                newItems.Add(item);
            }

            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                PrepareListViewCollection(newItems);
                CopyWgItemToClipboard(iWeightData.NetWeight);
            });

            return newId != Guid.Empty;
        }

        public void ClearResult()
        {
            this.SearchResult.Clear();
        }

    }
}
