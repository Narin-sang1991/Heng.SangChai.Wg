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
    /// Interaction logic for AboutManagerView.xaml
    /// </summary>
    public partial class AboutManagerView : Window
    {
        public AboutManagerView()
        {
            InitializeComponent();
        }

        private void ctlOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }

}
