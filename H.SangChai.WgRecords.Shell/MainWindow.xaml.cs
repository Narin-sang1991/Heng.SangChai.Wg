using H.SangChai.WgRecords.Shell.ViewModels;
using System.Windows;

namespace H.SangChai.WgRecords.Shell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void radDocking_PreviewClose(object sender, Telerik.Windows.Controls.Docking.StateChangeEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            vm.CloseCommand.Execute(e);
            e.Handled = true;
        }
    }
}
