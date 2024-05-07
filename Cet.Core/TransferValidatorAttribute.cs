using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Reflection;

namespace Cet.Core
{

    public class TransferValidatorAttribute : ValueValidatorAttribute
    {
        Type destination;
        String property;
        String ruleSet;


        public TransferValidatorAttribute(Type destination, String property) : this(destination, property, string.Empty)
        {
        }

        public TransferValidatorAttribute(Type destination, String property, String ruleSet)
        {
            this.destination = destination;
            this.property = property;
            this.ruleSet = ruleSet;
        }

        protected override Microsoft.Practices.EnterpriseLibrary.Validation.Validator DoCreateValidator(Type targetType)
        {
            return GetPropertyValidator(destination.GetProperty(property));
        }

        private Validator GetPropertyValidator(PropertyInfo prop)
        {
            var source = ValidationSpecificationSource.All;
            var builder = new FixedPropertyMemberValueAccessBuilder(prop);
            Validator validator = null;
            validator = PropertyValidationFactory.GetPropertyValidator(
             destination, prop, ruleSet, source, builder);
            if (validator == null) validator = new DummyValidator();
            return validator;
        }
    }

    public class FixedPropertyMemberValueAccessBuilder : MemberValueAccessBuilder
    {
        private readonly PropertyInfo property;

        public FixedPropertyMemberValueAccessBuilder(PropertyInfo property)
        {
            this.property = property;
        }

        protected override ValueAccess DoGetFieldValueAccess(FieldInfo fieldInfo)
        {
            throw new NotSupportedException();
        }

        protected override ValueAccess DoGetMethodValueAccess(MethodInfo methodInfo)
        {
            throw new NotSupportedException();
        }

        protected override ValueAccess DoGetPropertyValueAccess(PropertyInfo propertyInfo)
        {
            if (this.property == propertyInfo)
            {
                return new ExternalValueValueAccess();
            }

            throw new NotSupportedException();
        }
    }

    public class ExternalValueValueAccess : ValueAccess
    {
        public ExternalValueValueAccess()
        { }

        public override bool GetValue(object source, out object value, out string valueAccessFailureMessage)
        {
            value = source;
            valueAccessFailureMessage = null;
            return true;
        }

        public override string Key
        {
            get { return ""; }
        }
    }
}
