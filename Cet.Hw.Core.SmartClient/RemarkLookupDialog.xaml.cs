using Cet.Hw.Core.SmartClient.ViewModels;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cet.Hw.Core.SmartClient
{
    /// <summary>
    /// Interaction logic for RemarkLookupDialog.xaml
    /// </summary>
    public partial class RemarkLookupDialog : Window
    {

        private IUnityContainer container;

        public RemarkLookupDialog(IUnityContainer container)
        {
            InitializeComponent();
            this.container = container;
            var viewModel = this.container.Resolve<RemarkEditorVM>();
            this.DataContext = viewModel;
        }

        private void ctlOKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void ctlCancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

    }
}