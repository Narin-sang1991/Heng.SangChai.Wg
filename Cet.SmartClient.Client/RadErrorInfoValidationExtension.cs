using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls.Data;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Cet.SmartClient.Client
{
    public static class RadErrorInfoValidationExtension
    {
        public static void Add(this ObservableCollection<ErrorInfo> errors, ValidationResults addingResults)
        {
            foreach (ValidationResult r in addingResults)
                errors.Add(new ErrorInfo() { ErrorContent = r.Message, SourceFieldDisplayName = r.Key });
        }
    }
}
