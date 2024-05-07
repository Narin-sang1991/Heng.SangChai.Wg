//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Cet.Core
{
    /// <summary>
    /// Logs a failure when validating a <see langword="null"/> reference.
    /// </summary>
    [ConfigurationElementType(typeof(DummyValidatorData))]
    public class DummyValidator : ValueValidator
    {
        public DummyValidator()
            : base("", "", false)
        { }

        public override void DoValidate(object objectToValidate,
            object currentTarget,
            string key,
            ValidationResults validationResults)
        {
            return;
        }

        protected override string DefaultNonNegatedMessageTemplate
        {
            get { return ""; }
        }

        protected override string DefaultNegatedMessageTemplate
        {
            get { return ""; }
        }
    }

    public class DummyValidatorData : ValidatorData
    {
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new DummyValidator();
        }
    }
}
