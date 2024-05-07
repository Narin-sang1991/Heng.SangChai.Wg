using Cet.Hw.Core.SmartClient.ViewModels;
using Cet.SmartClient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Data;
using Microsoft.Practices.Unity;

namespace Cet.Hw.Core.SmartClient
{
    /// <summary>
    /// Interaction logic for UserEditorView.xaml
    /// </summary>
    public partial class UserEditorView : EditorViewBase
    {
        public UserEditorView()
        {
            InitializeComponent();
        }

        private void OnCurrentTreeItemChanged(object sender, EventArgs e)
        {
            if (ctlUserEditor.CurrentItem == null) return;

            var editorVM = ctlUserEditor.CurrentItem as UserEditorVM;

            if (!editorVM.HasOriginalSource)
                ctlUserEditor.BeginEdit();
            editorVM.Activate();
        }
    }
}