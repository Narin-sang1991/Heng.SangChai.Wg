using Cet.Hw.Core.SmartClient.ViewModels;
using Cet.SmartClient.Client;
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
    /// Interaction logic for UserSearchView.xaml
    /// </summary>
    public partial class UserSearchView : TreeViewBase
    {
        public TreeSearchVMBase UserSearchVM
        {
            get { return this.DataContext as TreeSearchVMBase; }
        }

        public UserSearchView()
        {
            InitializeComponent();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UserSearchVM.PrepareRootNodes();
        }

    }
}