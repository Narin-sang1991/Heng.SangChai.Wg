using Cet.SmartClient.Client;
using H.SangChai.WgRecords.Shell.Models;
using H.SangChai.WgRecords.Shell.ViewModels;
using System.Linq;
using Telerik.Windows.Controls;

namespace H.SangChai.WgRecords.Shell
{
    /// <summary>
    /// Interaction logic for PartTransferItemSearchView.xaml
    /// </summary>
    public partial class PartTransferItemSearchView : SearchViewBase
    {
        public PartTransferItemSearchView()
        {
            InitializeComponent();
        }

        private void ctlPartTransferReceiveSearchResultGridView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var vm = this.DataContext as PartTransferItemSearchVM;
            var eventSelected = ((RadGridView)e.Source).SelectedCells.FirstOrDefault().Item as MeasuringItemData;
            if (eventSelected == null) return;

            vm.SetCurrentClipboard(eventSelected);
        }
    }
}
