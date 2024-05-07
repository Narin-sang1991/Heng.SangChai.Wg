using Cet.Hw.Core.SmartClient.ViewModels;
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
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;


namespace Cet.Hw.Core.SmartClient
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginVM LoginVM { get { return this.DataContext as LoginVM; } }

        public IUnityContainer Container { get; set; }

        public LoginView(IUnityContainer container)
        {
            InitializeComponent();
            Container = container;
        }

        private void ctlCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ctlOK_Click(object sender, RoutedEventArgs e)
        {
            LoginResultCode result = LoginVM.CheckLogin();
            if (result == LoginResultCode.Success)
            {
                this.Close();
            }
            else if (result == LoginResultCode.Invalid)
            {
                ctlLoginForm.ValidationSummary.Errors.Clear();
                ctlLoginForm.ValidationSummary.Errors.Add(new Telerik.Windows.Controls.Data.ErrorInfo() { ErrorContent = "Invalid username and password" });
            }
            else if (result == LoginResultCode.RequiredOU)
            {
                ctlLoginForm.ValidationSummary.Errors.Clear();
                ctlLoginForm.ValidationSummary.Errors.Add(new Telerik.Windows.Controls.Data.ErrorInfo() { ErrorContent = "Please enter organization unit." });
            }

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = e.Source as PasswordBox;
            LoginVM.PasswordInput = passwordBox.Password;
        }

        private void ctlLoginForm_CurrentItemChanged(object sender, EventArgs e)
        {
            ctlLoginForm.BeginEdit();
        }

        private void ctlLoginForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {

        }


    }
}
