using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using System.ComponentModel;

namespace Cet.SmartClient.Client
{
    /// <summary>
    /// When set original source the vm will just load in current v level but not child nodes. 
    /// Call FullLoad to activate PrepareChildNode to run.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RootDocEditorVMBase<T> : EditorVMBase<T>, IOpenEditorVM<Guid>
        where T : new()
    {
        private Guid? id;
        public Guid? Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        protected T SavedDocument { get; set; }
        public RootDocEditorVMBase()
            : this(null)
        { }

        public RootDocEditorVMBase(IUnityContainer container)
            : base(container)
        { }

        public override void BeginEdit()
        {
            this.IsEditing = true;
            foreach (DocEditorVMBase editor in GetChildNode().OfType<DocEditorVMBase>())
                editor.BeginEdit();
        }

        public override void CancelEdit()
        {
            if (!this.IsEditing) return;

            this.IsEditing = false;
            if (HasOriginalSource)
                this.LoadOriginalSource(OriginalSource);

            //load everything back from original source
            FullLoad();
            //just to restore IsEditing flag
            foreach (DocEditorVMBase editor in GetChildNode().OfType<DocEditorVMBase>())
                editor.EndEdit();

            OnEditCancelled();
        }

        public override void EndEdit()
        {
            if (!this.IsEditing) return;

            this.IsEditing = false;
            OriginalSource = SavedDocument;
            //just to restore IsEditing flag
            foreach (DocEditorVMBase editor in GetChildNode().OfType<DocEditorVMBase>())
                editor.EndEdit();
        }

        public void Load(Guid id)
        {
            DispatcherFrame frame = new DispatcherFrame();
            RunWorkerCompletedEventArgs e = null;
            System.Globalization.CultureInfo uiOldCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            var bw = new BackgroundWorker();
            bw.DoWork += (sender, arg) =>
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = uiOldCulture;
                T item = LoadInternal(id);
                arg.Result = item;
            };

            bw.RunWorkerCompleted += (sender, arg) =>
            {
                IsBusy = false;
                frame.Continue = false;
                e = arg;
                if (e.Error != null) return;
                Id = id;
                OriginalSource = (T)arg.Result;
                FullLoad();

                RefreshHeader(OpenModeType.Update);
            };

            IsBusy = true;
            bw.RunWorkerAsync();
            Dispatcher.PushFrame(frame);
            if (e != null && e.Error != null) throw e.Error;
        }
        public override async Task ReloadAsync()
        {
            await LoadAsync(Id.Value);
        }

        public virtual async Task LoadAsync(Guid id)
        {
            Id = id;
            IsBusy = true;

            System.Globalization.CultureInfo uiOldCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;

            try
            {
                T item = await Task.Run(() =>
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = uiOldCulture;
                    return LoadInternal(id);
                });
                OriginalSource = item;
                FullLoad();
            }
            finally
            {
                IsBusy = false;
            }
            RefreshHeader(OpenModeType.Update);
        }

        protected virtual T LoadInternal(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual void RefreshHeader(OpenModeType mode)
        {
            throw new NotImplementedException();
        }
    }
}
