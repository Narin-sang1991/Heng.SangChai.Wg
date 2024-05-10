using Cet.SmartClient.Client;
using H.SangChai.WgRecords.Shell.ViewModels;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using WPFLocalizeExtension.Extensions;

namespace H.SangChai.WgRecords.Shell.Commands
{
    public class WgRecordEditorCommand : MenuCommand
    {
        private readonly IRegionManager regionManager;
        public WgRecordEditorCommand(IUnityContainer container, IRegionManager regionManager)
        : base(container, MenuResources.WgRecord)
        {
            this.regionManager = regionManager;
        }

        public override void Execute(object parameter)
        {
            IRegion region = regionManager.Regions["ContentRegion"];
            string viewName = "WgRecordEditorView";
            object view = region.GetView(viewName);

            if (view == null)
            {
                var usercontrol = this.container.Resolve<WgRecordEditorView>();
                var viewModel = this.container.Resolve<WgRecordEditorVM>();
                viewModel.Header = new LocTextExtension() { Key = "DISPLAY_HEADER", Dict = "WgRecordEditorView", Assembly = "H.SangChai.WgRecords.Shell" };
                usercontrol.DataContext = viewModel;
                view = region.AddView(usercontrol, viewName);
            }
            region.Activate(view);
        }
    }
}