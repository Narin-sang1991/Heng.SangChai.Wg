using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Cet.SmartClient.Client
{
    public static class RadGridViewCellValidationResultsExtension
    {
        public static void Add(this IList<GridViewCellValidationResult> validationResults, ValidationResults addingResults)
        {
            foreach(ValidationResult r in addingResults)
                validationResults.Add(new GridViewCellValidationResult() {ErrorMessage = r.Message, PropertyName = r.Key});
        }
    }
}
