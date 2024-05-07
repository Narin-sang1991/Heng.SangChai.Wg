using Cet.Hw.Core.SmartClient.ViewModels;
using Cet.SmartClient.Client;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Cet.Hw.Core.SmartClient
{
    /// <summary>
    /// Interaction logic for UserItemManagerView.xaml
    /// </summary>
    public partial class UserItemManagerView : InlineEditSearchViewBase
    {
        public UserItemManagerView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            var box = sender as PasswordBox;
            var editorVM = box.DataContext as UserItemEditorVM;
            editorVM.Password = box.Password;
        }
    }
}
