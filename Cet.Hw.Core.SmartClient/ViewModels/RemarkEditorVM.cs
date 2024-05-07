using Cet.Hw.Core.AppServiceContract;
using Cet.SmartClient.Client;
using Microsoft.Practices.Unity;


namespace Cet.Hw.Core.SmartClient.ViewModels
{
    public class RemarkEditorVM : DocEditorVMBase<RemarkData>
    {

        public RemarkEditorVM() : base() { }

        public RemarkEditorVM(IUnityContainer container)
            : base(container)
        {
            this.Header = Cet.Hw.Core.SmartClient.Resources.RemarkEditorView.DISPLAY_HEADER;
        }
     
        private string remark;
        public string Remark
        {
            get { return remark; }
            set
            {
                remark = value;
                OnPropertyChanged("Remark");
                OnPropertyChanged("HasRemarkValue");
            }
        }

        public bool HasRemarkValue
        {
            get { return !string.IsNullOrWhiteSpace(Remark); }
        }

        public override void SaveOriginalSource(RemarkData originalSource)
        {
            originalSource.Value = Remark;
        }

        public override void LoadOriginalSource(RemarkData originalSource)
        {

            Remark = originalSource.Value;
        }


    }

}

