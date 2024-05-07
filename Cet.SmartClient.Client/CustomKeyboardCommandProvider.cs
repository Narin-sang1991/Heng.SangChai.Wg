using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls.Data.DataForm;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Data;
using System.Windows.Input;

namespace Cet.SmartClient.Client
{
    public class CustomKeyboardCommandProvider : DataFormCommandProvider
    {
        public CustomKeyboardCommandProvider()
            : base(null)
        {

        }

        public CustomKeyboardCommandProvider(RadDataForm dataForm)
            : base(dataForm)
        {
            this.DataForm = dataForm;
        }
        public override List<DelegateCommandWrapper> ProvideCommandsForKey(KeyEventArgs args)
        {
            List<DelegateCommandWrapper> actionsToExecute = base.ProvideCommandsForKey(args);
            //if (args.Key == Key.Right)
            //{
            //    actionsToExecute.Clear();
            //    actionsToExecute.Add(new DataFormDelegateCommandWrapper(RadDataFormCommands.MoveCurrentToNext, this.DataForm));
            //    actionsToExecute.Add(new DataFormDelegateCommandWrapper(RadDataFormCommands.BeginEdit, this.DataForm));
            //}
            //if (args.Key == Key.Left)
            //{
            //    actionsToExecute.Clear();
            //    actionsToExecute.Add(new DataFormDelegateCommandWrapper(RadDataFormCommands.MoveCurrentToPrevious, this.DataForm));
            //    actionsToExecute.Add(new DataFormDelegateCommandWrapper(RadDataFormCommands.BeginEdit, this.DataForm));
            //}

            if (args.Key == Key.Escape || args.Key == Key.Enter)
            {
                var item = actionsToExecute.Where(t => t.InvocationKey == args.Key).FirstOrDefault();
                //actionsToExecute.Clear();
                actionsToExecute.Remove(item);
            }

            if (actionsToExecute.Count > 0)
            {
                actionsToExecute.Add(new DataFormDelegateCommandWrapper(new Action(() => { this.DataForm.AcquireFocus(); }), 100, this.DataForm));
                args.Handled = true;
            }
            return actionsToExecute;
        }
    }
}
