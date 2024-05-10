using Cet.SmartClient.Client;
using Microsoft.Practices.Unity;
using System;

namespace H.SangChai.WgRecords.Shell.IOModels
{
    public class RS232SerialPortInput : RS232SerialPortControlBase
    {

        public DelegateCommand ReConnectCommand { get; set; }

        public RS232SerialPortInput() : base() { }

        public RS232SerialPortInput(IUnityContainer iContainer)
            : base(iContainer)
        {
            #region Require on shell unity.tranform.config
            var confComportName = Container.Resolve<string>("ComportNameInput");
            var confStartText = Container.Resolve<string>("ComportDataStartText");
            var confEndText = Container.Resolve<string>("ComportDataEndText");
            var confDataStartPoint = Container.Resolve<byte>("ComportDataStartPoint");
            var confDataLength = Container.Resolve<byte>("ComportDataLength");
            this.ErrorValue = Container.Resolve<decimal>("ErrorValueReceive");
            #endregion
            ResetWeight();
            ReConnectCommand = new DelegateCommand(ConnectPort);
            SetComport(confComportName, confStartText, confEndText, confDataStartPoint, confDataLength);

            autosaveCouting = 0;
            minWgAllowSave = Container.Resolve<decimal>("MinWgAllowAutoSave");
            AutoSaveSecLimitCounting = Container.Resolve<int>("AutoSaveLimitCounting");
        }

        public event EventHandler WeightAutoSave;
        public event EventHandler WeightZero;
        private int autosaveCouting;
        private decimal minWgAllowSave;
        public int AutoSaveSecLimitCounting { get; set; }

        protected override void VerifyWeightChange()
        {
            if (Weight > 0 && (Weight <= lastestWeight - ErrorValue
                                  || Weight >= lastestWeight + ErrorValue
                                  || lastestWeight == 0)
                                  )
            {
                if (autosaveCouting >= AutoSaveSecLimitCounting && Weight > minWgAllowSave)
                {
                    if (WeightAutoSave != null)
                        WeightAutoSave(this, null);
                    lastestWeight = Weight;
                    autosaveCouting = 0;
                }
                autosaveCouting++;
            }
            else
                autosaveCouting = 0;

            if (Weight == 0 && lastestWeight != 0 && WeightZero != null)
                WeightZero(this, null);
        }

    }
}
