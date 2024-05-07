using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Practices.Unity;
using System.Windows;
using System.Windows.Data;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Reflection;

namespace Cet.SmartClient.Client
{
    public class DocEditorVMBase : TabViewModel, IDataErrorInfo
    {
        private bool isEditing;
        public bool IsEditing
        {
            get { return isEditing; }
            protected set
            {
                isEditing = value;
                OnPropertyChanged("IsEditing");
                OnPropertyChanged("IsNotEditing");
            }
        }

        public bool IsNotEditing
        {
            get { return !isEditing; }
        }


        public virtual void BeginEdit()
        {
            this.IsEditing = true;

            foreach (string key in ChildNodeGroups.Keys)
            {
                foreach (DocEditorVMBase child in ChildNodeGroups[key])
                {
                    child.BeginEdit();
                }
            }
        }

        public virtual void EndEdit()
        {
            if (!this.IsEditing) return;

            this.IsEditing = false;

            foreach (string key in ChildNodeGroups.Keys)
            {
                foreach (DocEditorVMBase child in ChildNodeGroups[key])
                {
                    child.EndEdit();
                }
            }
        }
        public DocEditorVMBase() : this(null)
        {
        }

        public DocEditorVMBase(IUnityContainer container) :base(container)
        {
        }

        string IDataErrorInfo.Error
        {
            get { return string.Empty; }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (string.IsNullOrEmpty(columnName)) return string.Empty;

                var prop = this.GetType().GetProperty(columnName);
                return this.GetErrorInfo(prop);
            }
        }

        protected virtual string GetErrorInfo(PropertyInfo prop)
        {
            var validator = this.GetPropertyValidator(prop);

            if (validator != null)
            {
                var results = validator.Validate(this);

                if (!results.IsValid)
                {
                    return string.Join(" ",
                        results.Select(r => r.Message).ToArray());
                }
            }

            return string.Empty;
        }

        private Validator GetPropertyValidator(PropertyInfo prop)
        {
            string ruleset = string.Empty;
            var source = ValidationSpecificationSource.Attributes;
            var builder = new ReflectionMemberValueAccessBuilder();
            Validator validator = null;
            validator = PropertyValidationFactory.GetPropertyValidator(
               this.GetType(), prop, ruleset, source, builder);
            return validator;
        }

        //public abstract void SaveAll();
    }

    public abstract class DocEditorVMBase<T> : DocEditorVMBase where T : new()
    {
        public DocEditorVMBase() : base()
        { 
        }

        public DocEditorVMBase(IUnityContainer container) : base(container)
        {
        }

        public virtual void LoadOriginalSource(T originalSource) { }

        public virtual void SaveOriginalSource(T originalSource) { }

    }
}
