using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.Unity;
using System.Collections.ObjectModel;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cet.SmartClient.Client
{
    public class EditorVMBase<T> : EditorVMBase where T : new()
    {
        public event EventHandler EditCancelled;

        public EditorVMBase()
            : this(null)
        { }

        public EditorVMBase(IUnityContainer container)
            : base(container)
        { }

        private T originalSource;
        public T OriginalSource
        {
            get { return originalSource; }
            set
            {
                originalSource = value;

                if (originalSource == null)
                    HasOriginalSource = false;
                else
                {
                    HasOriginalSource = true;
                    LoadOriginalSource(originalSource);
                }
            }
        }

        public override void BeginEdit()
        {
            this.IsEditing = true;
        }

        public override void CancelEdit()
        {
            if (!this.IsEditing) return;

            this.IsEditing = false;
            if (HasOriginalSource)
                this.LoadOriginalSource(OriginalSource);

            OnEditCancelled();
        }

        public override void EndEdit()
        {
            if (!this.IsEditing) return;

            this.IsEditing = false;
            var temp = new T();
            this.SaveOriginalSource(temp);
            OriginalSource = temp;
            FullLoad();
        }

        protected void OnEditCancelled()
        {
            if (EditCancelled != null)
                EditCancelled(this, EventArgs.Empty);
        }

        public override void Save()
        {
            DispatcherFrame frame = new DispatcherFrame();
            RunWorkerCompletedEventArgs e = null;
            System.Globalization.CultureInfo uiOldCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;

            var bw = new BackgroundWorker();
            bw.DoWork += (s, arg) =>
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = uiOldCulture;
                SaveInternal();
            };

            bw.RunWorkerCompleted += (s, arg) =>
            {
                frame.Continue = false;
                IsBusy = false;
                e = arg;
            };

            IsBusy = true;
            bw.RunWorkerAsync();
            Dispatcher.PushFrame(frame);
            if (e != null && e.Error != null) throw e.Error;
        }

        public override async Task SaveAsync()
        {
            IsBusy = true;
            System.Globalization.CultureInfo uiOldCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;

            await Task.Run(() =>
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = uiOldCulture;
                SaveInternal();
            });
            IsBusy = false;
        }

        protected virtual void SaveInternal()
        {
            throw new NotImplementedException();
        }

        public override void Remove()
        {
            DispatcherFrame frame = new DispatcherFrame();
            RunWorkerCompletedEventArgs e = null;
            System.Globalization.CultureInfo uiOldCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            var bw = new BackgroundWorker();

            bw.DoWork += (s, arg) =>
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = uiOldCulture;
                RemoveInternal();
            };

            bw.RunWorkerCompleted += (s, arg) =>
            {
                frame.Continue = false;
                IsBusy = false;
                e = arg;
                if (e.Error != null) return;
            };

            IsBusy = true;
            bw.RunWorkerAsync();
            Dispatcher.PushFrame(frame);
            if (e != null && e.Error != null) throw e.Error;
            NotifyDeletedItem();
        }

        public override async Task RemoveAsync()
        {
            IsBusy = true;
            System.Globalization.CultureInfo uiOldCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            try
            {
                await Task.Run(() =>
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = uiOldCulture;
                    RemoveInternal();
                });
            }
            finally
            {
                IsBusy = false;
            }
            NotifyDeletedItem();
        }

        protected virtual void RemoveInternal()
        {
            throw new NotImplementedException();
        }

        protected virtual void LoadOriginalSource(T originalSource)
        {
            throw new NotImplementedException();
        }

        protected virtual void SaveOriginalSource(T originalSource)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class EditorVMBase : EditableContainerBase, IEditableObject, IDataErrorInfo
    {
        public EditorVMBase()
            : this(null)
        {

        }

        public EditorVMBase(IUnityContainer container)
            : base(container)
        {
        }

        public event EventHandler DeletedItem;

        private bool hasOriginalSource;
        public bool HasOriginalSource
        {
            get { return hasOriginalSource; }
            protected set
            {
                hasOriginalSource = value;
                OnPropertyChanged("HasOriginalSource");
            }
        }

        protected override void FullLoad()
        {
            if (HasOriginalSource)
                base.FullLoad();
        }

        public abstract void Save();
        public virtual Task SaveAsync()
        {
            throw new NotImplementedException();
        }
        public abstract void Remove();

        public virtual Task RemoveAsync()
        {
            throw new NotImplementedException();
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
                    string msg = string.Join(" ",
                        results.Select(r => r.Message).ToArray());
                    return msg;
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

        public abstract void BeginEdit();
        public abstract void CancelEdit();
        public abstract void EndEdit();


        /// <summary>
        /// Support reloading in subclass.
        /// </summary>
        /// <returns></returns>
        public virtual Task ReloadAsync() { throw new NotImplementedException(); }

        private ICommand reloadAsyncCommand;
        public ICommand ReloadAsyncCommand
        {
            get
            {
                if (reloadAsyncCommand == null)
                    reloadAsyncCommand = new DelegateCommand(async () => { await ReloadAsync(); }, null);

                return reloadAsyncCommand;
            }
        }

        public void NotifyDeletedItem()
        {
            if (DeletedItem != null)
                DeletedItem(this, EventArgs.Empty);
        }
    }
}
