using Cet.Core.Logging;
using Cet.SmartClient.Client;
using H.SangChai.WgRecords.Shell.IOModels;
using H.SangChai.WgRecords.Shell.Models;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFLocalizeExtension.Extensions;

namespace H.SangChai.WgRecords.Shell.ViewModels
{
    public class WgRecordEditorVM : TransactionIOEditorVM
    {
        public WgRecordEditorVM() : base() { }

        public WgRecordEditorVM(IUnityContainer container)
                : base(container)
        {
            RS232Input = Container.Resolve<RS232SerialPortInput>();
            RS232Input.WeightAutoSave += NotifyWeightItemChanged;

            ReConnectAllPortCommand = new DelegateCommand(ReConnectAllPort);
            SaveStateColor = System.ConsoleColor.White.ToString();

            ItemSearchVM = Container.Resolve<PartTransferItemSearchVM>();
            PrepareChildVMs();

            this.Id = Guid.NewGuid();
            this.BeginEdit();

            ManualSaveItemCommand = new DelegateCommand(ManualSaveItem);
            TestSaveItemCommand = new DelegateCommand(TestSaveItem);
        }

        #region Propertie
        public DelegateCommand ReConnectAllPortCommand { get; set; }
        public DelegateCommand ManualSaveItemCommand { get; set; }
        public DelegateCommand TestSaveItemCommand { get; set; }

        private RS232SerialPortInput rs232Input;
        public RS232SerialPortInput RS232Input
        {
            get { return rs232Input; }
            set
            {
                rs232Input = value;
                OnPropertyChanged("ComportName");
                OnPropertyChanged("RS232Input");
            }
        }

        private PartTransferItemSearchVM itemSearchVM;
        public PartTransferItemSearchVM ItemSearchVM
        {
            get { return itemSearchVM; }
            set
            {
                itemSearchVM = value;
                OnPropertyChanged("ItemSearchVM");
                OnPropertyChanged("TotalNetWeight");
                OnPropertyChanged("TotalTareWeight");
                OnPropertyChanged("TotalGrossWeight");
            }
        }

        public decimal TotalNetWeight
        {
            get
            {
                var result = ItemSearchVM != null ? Math.Round(ItemSearchVM.SearchResult.Sum(t => t.NetWeight), 2, MidpointRounding.AwayFromZero) : 0;
                return result;
            }
        }

        public decimal TotalTareWeight
        {
            get
            {
                var result = ItemSearchVM != null ? Math.Round(ItemSearchVM.SearchResult.Sum(t => t.TareWeight), 2, MidpointRounding.AwayFromZero) : 0;
                return result;
            }
        }

        public decimal TotalGrossWeight
        {
            get
            {
                var result = TotalNetWeight + TotalTareWeight;
                result = Math.Round(result, 2, MidpointRounding.AwayFromZero);
                return result;
            }

        }
        #endregion

        #region Methods
        public override void PrepareChildVMs()
        {
            ItemSearchVM.Header = new LocTextExtension() { Key = "DISPLAY_HEADER", Dict = "PartTransferItemSearchView", Assembly = "H.SangChai.WgRecords.Shell" };
            AddChildNode(ItemSearchVM);
        }

        protected void NotifyWeightItemChanged(object sender, EventArgs args)
        {
            if (this.Id.HasValue) TriggerItemSearchVM(RS232Input.Weight);
        }

        protected void ManualSaveItem()
        {
            TriggerItemSearchVM(RS232Input.Weight);
        }

        protected void TestSaveItem()
        {
            TriggerItemSearchVM(85.15m);
        }

        protected void ReConnectAllPort()
        {
            RS232Input.CloseConnect();
            RS232Input.ConnectPort();
        }

        protected override void TriggerItemSearchVM(decimal iWeight)
        {
            var isSaveCompleted = false;
            try
            {
                var data = new WeightData();
                data.NetWeight = iWeight - this.DefaultTare;
                data.TareWeight = this.DefaultTare;
                isSaveCompleted = ItemSearchVM.SetWeight(data);
                ResponseResult(isSaveCompleted);
            }
            catch (Exception ex)
            {
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(ex.ToString() + "[" + RS232Input.WeightDisplay + "]", ApplicationLogCategory.General, (int)Priority.High, (int)GeneralApplicationLogEvent.DispatcherUnhandledException, System.Diagnostics.TraceEventType.Error);
            }
            System.Windows.Application.Current.Dispatcher.InvokeAsync((Action)(() =>
            {
                ResponseResult(isSaveCompleted);
            }));
        }

        protected override void NotifyPartItemChanged()
        {
            OnPropertyChanged("TotalNetWeight");
            OnPropertyChanged("TotalTareWeight");
            OnPropertyChanged("TotalGrossWeight");
        }

        protected void ResponseResult(bool isSaveCompleted)
        {
            if (isSaveCompleted)
            {
                if (SaveStateColor != System.ConsoleColor.Green.ToString())
                    SaveStateColor = System.ConsoleColor.Green.ToString();
                NotifyPartItemChanged();
            }
            else
            {
                if (SaveStateColor != System.ConsoleColor.Red.ToString())
                    SaveStateColor = System.ConsoleColor.Red.ToString();
                Reset();
            }
        }

        public async void Reset()
        {
            await Task.Delay(delayReset);
            RS232Input.ResetWeight();
            if (SaveStateColor != System.ConsoleColor.Yellow.ToString())
                SaveStateColor = System.ConsoleColor.Yellow.ToString();
        }
        #endregion 
    }
}
