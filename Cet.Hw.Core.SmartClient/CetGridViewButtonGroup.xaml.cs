using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for CetGridViewButtonGroup.xaml
    /// </summary>
    public partial class CetGridViewButtonGroup : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(CetGridViewButtonGroup), new UIPropertyMetadata(null));

        public IInputElement CommandTarget
        {
            get { return (IInputElement)base.GetValue(CommandTargetProperty); }
            set
            {
                base.SetValue(CommandTargetProperty, value);
            }
        }

        //private Visibility showNavigationButtons;
        //public Visibility ShowNavigationButtons
        //{
        //    get { return showNavigationButtons; }
        //    set
        //    {
        //        showNavigationButtons = value;
        //        OnPropertyChanged("ShowNavigationButtons");
        //    }
        //}

        private Visibility showCommitButtons;
        public Visibility ShowCommitButtons
        {
            get { return showCommitButtons; }
            set
            {
                showCommitButtons = value;
                OnPropertyChanged("ShowCommitButtons");
            }
        }

        private Visibility showAddButton;
        public Visibility ShowAddButton
        {
            get { return showAddButton; }
            set
            {
                showAddButton = value;
                OnPropertyChanged("ShowAddButton");
            }
        }

        private Visibility showEditButton;
        public Visibility ShowEditButton
        {
            get { return showEditButton; }
            set
            {
                showEditButton = value;
                OnPropertyChanged("ShowEditButton");
            }
        }

        private Visibility showDeleteButton;
        public Visibility ShowDeleteButton
        {
            get { return showDeleteButton; }
            set
            {
                showDeleteButton = value;
                OnPropertyChanged("ShowDeleteButton");
            }
        }

        private bool isAsync;

        public bool IsAsync
        {
            get { return isAsync; }
            set { isAsync = value; OnPropertyChanged("IsAsync"); OnPropertyChanged("ShowSearchButton"); OnPropertyChanged("ShowSearchAsyncButton"); }
        }


        private bool isNotShowSearchButton;
        public bool IsNotShowSearchButton
        {
            get { return isNotShowSearchButton; }
            set { isNotShowSearchButton = value; OnPropertyChanged("IsAsync"); OnPropertyChanged("ShowSearchButton"); OnPropertyChanged("ShowSearchAsyncButton"); }
        }

        public Visibility ShowSearchButton
        {
            get { return isAsync || IsNotShowSearchButton ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Visibility ShowSearchAsyncButton
        {
            get { return isAsync ? Visibility.Visible : Visibility.Collapsed; }
        }

        public CetGridViewButtonGroup()
        {
            InitializeComponent();
        }
    }
}
