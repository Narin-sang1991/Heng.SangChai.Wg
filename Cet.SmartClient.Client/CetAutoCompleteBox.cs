using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Telerik.Windows.Controls;

namespace Cet.SmartClient.Client
{
    public class CetAutoCompleteBox : RadAutoCompleteBox
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private bool isHandled = true;
        private bool isDeleting;

        public CetAutoCompleteBox()
            : base()
        {
            this.timer.Interval = TimeSpan.FromSeconds(0.1);
            this.timer.Tick += OnTimerTick;

            this.SearchTextChanged += OnAutoCompleteBoxSearchTextChanged;
            this.Populating += OnAutoCompleteBoxPopulating;
        }

        public readonly static DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand),
            typeof(CetAutoCompleteBox), new FrameworkPropertyMetadata());

        public readonly static DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandProperty", typeof(object),
            typeof(CetAutoCompleteBox), new FrameworkPropertyMetadata());

        public readonly static DependencyProperty CommandTargetProperty;

        public ICommand Command
        {
            get
            {
                return (ICommand)base.GetValue(CetAutoCompleteBox.CommandProperty);
            }
            set { base.SetValue(CetAutoCompleteBox.CommandProperty, value); }
        }

        public object CommandParameter
        {
            get
            {
                return base.GetValue(CetAutoCompleteBox.CommandParameterProperty);
            }
            set { base.SetValue(CetAutoCompleteBox.CommandParameterProperty, value); }
        }

        private void OnAutoCompleteBoxSearchTextChanged(object sender, EventArgs e)
        {
            if (this.timer != null)
            {
                this.timer.Stop();
                this.timer.Start();
            }

            this.isHandled = true;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            this.timer.Stop();
            this.isHandled = false;
            //call command
            if (Command != null)
                Command.Execute(this.SearchText);
        }


        void OnAutoCompleteBoxPopulating(object sender, CancelRoutedEventArgs e)
        {
            e.Cancel = isHandled;
        }
    }
}
