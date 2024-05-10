using Cet.SmartClient.Client;
using H.SangChai.WgRecords.Shell.Models;
using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;

namespace H.SangChai.WgRecords.Shell.ViewModels
{
    public class TransactionIOEditorVM : EditorVMBase<MeasuringData>
    {

        public TransactionIOEditorVM() : base() { }

        public TransactionIOEditorVM(IUnityContainer container)
                : base(container)
        {
            delayReset = Container.Resolve<int>("DelayResetStanbyInput");
            DefaultTare = 0m;
        }

        #region Properties

        private Guid? id;
        public Guid? Id
        {
            get { return id; }
            protected set
            {
                id = value;
            }
        }

        //public bool HasId { get { return Id.HasValue; } }

        //private DateTimeOffset? movementDate;
        //public DateTimeOffset? MovementDate
        //{
        //    get { return movementDate; }
        //    private set
        //    {
        //        movementDate = value;
        //    }
        //}

        private decimal defaultTare;
        public decimal DefaultTare
        {
            get { return defaultTare; }
            set
            {
                defaultTare = value;
                OnPropertyChanged("DefaultTare");
            }
        }

        private string saveStateColor;
        public string SaveStateColor
        {
            get { return saveStateColor; }
            protected set
            {
                if (saveStateColor == value) return;

                saveStateColor = value;
                OnPropertyChanged("SaveStateColor");
            }
        }

        protected int delayReset { get; set; } 
        #endregion

        #region Action

        #endregion

        #region Method
        protected virtual void TriggerItemSearchVM(decimal iWeight)
        {
            throw new NotImplementedException();
        }

        protected virtual void NotifyPartItemChanged()
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
