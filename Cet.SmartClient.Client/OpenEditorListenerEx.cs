using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;
using System.Windows.Controls;

namespace Cet.SmartClient.Client
{
    public enum OpenModeType
    {
        New, Update,Ready
    }

    public interface IOpenEditorVM<Key>
    {
        void Load(Key id);
        void Activate();
        event EventHandler EditCancelled;
        event EventHandler DeletedItem;
        object Header { get; set; }
        void RefreshHeader(OpenModeType mode);
    }

    public interface IGeneralOpenView
    {
        object DataContext { get; set; }
        object MainDataForm { get; }
    }

    public class GeneralOpenPayLoad<PkType, ParamType>
        //where PkType : new()
        where ParamType : new()
    {
        public PkType Id { get; set; }
        /// <summary>
        /// TypeName for resolving view model as registered in unity container with this name. 
        /// </summary>
        public string TypeName { get; set; }
        public OpenModeType OpenMode { get; set; }
        public ParamType Param { get; set; }
    }

    public class OpenEditorListenerEx<PkType, ParamType, OpenEventType>
        where ParamType : new()
        where OpenEventType : CompositePresentationEvent<GeneralOpenPayLoad<PkType, ParamType>>, new()
    {
        protected Dictionary<PkType, string> ViewNames { get; set; }
        protected Dictionary<object, string> ViewNews { get; set; }

        protected OpenModeType OpenMode { get; set; }

        private SubscriptionToken subscriptionTokenOpen;

        [Dependency]
        public IUnityContainer Container { get; set; }

        [Dependency]
        public IEventAggregator EventAggregator { get; set; }

        public OpenEditorListenerEx()
        {
            ViewNames = new Dictionary<PkType, string>();
            ViewNews = new Dictionary<object, string>();
        }

        public void Initialize()
        {
            if (subscriptionTokenOpen != null)
            {
                EventAggregator.GetEvent<OpenEventType>().Unsubscribe(subscriptionTokenOpen);
            }
            subscriptionTokenOpen = EventAggregator.GetEvent<OpenEventType>().Subscribe(Open);
        }

        public virtual void Open(GeneralOpenPayLoad<PkType, ParamType> payLoad)
        {

        }

        protected virtual void Cancel(object sender, EventArgs e)
        {
            CloseTab(sender);
        }

        protected virtual void DeletedItemCloseTab(object sender, EventArgs e)
        {
            CloseTab(sender);
        }

        protected void CloseTab(object viewModel)
        {
            if (!ViewNews.ContainsKey(viewModel)) return;

            IRegionManager regionManager = Container.Resolve<IRegionManager>();
            IRegion region = regionManager.Regions["ContentRegion"];
            object view = region.GetView(ViewNews[viewModel]);
            if (view != null)
            {
                region.Remove(view);
                ViewNews.Remove(viewModel);
            }
        }
    }

    public class GeneralOpenEditorListenerEx<EditorVMType, EditorViewType, OpenEventType, PkType, ParamType> : OpenEditorListenerEx<PkType, ParamType, OpenEventType>
        where ParamType : new()
        where EditorVMType : IOpenEditorVM<PkType>
        where EditorViewType : IGeneralOpenView
        where OpenEventType : CompositePresentationEvent<GeneralOpenPayLoad<PkType, ParamType>>, new()
    {
        public GeneralOpenEditorListenerEx()
            : base()
        {

        }

        public virtual void GetParameter(EditorVMType editorVM, ParamType parameter)
        {

        }

        public override void Open(GeneralOpenPayLoad<PkType, ParamType> payLoad)
        {
            OpenMode = payLoad.OpenMode;

            IRegionManager regionManager = Container.Resolve<IRegionManager>();
            IRegion region = regionManager.Regions["ContentRegion"];

            object view = null;
            EditorVMType viewModel;
            EditorViewType usercontrol;
            string viewName = string.Empty;

            //resolve view model & view
            if (string.IsNullOrEmpty(payLoad.TypeName))
            {
                viewModel = this.Container.Resolve<EditorVMType>();
                usercontrol = this.Container.Resolve<EditorViewType>();
            }
            else
            {
                viewModel = this.Container.Resolve<EditorVMType>(payLoad.TypeName);
                usercontrol = this.Container.Resolve<EditorViewType>(payLoad.TypeName);
            }

            if (viewModel != null)
            {
                GetParameter(viewModel, payLoad.Param);
                viewModel.DeletedItem += DeletedItemCloseTab;
            }

            if (payLoad.OpenMode == OpenModeType.New)
            {
                viewName = Guid.NewGuid().ToString();

                viewModel.Activate();
                viewModel.RefreshHeader(payLoad.OpenMode);
                viewModel.EditCancelled += Cancel;
                ViewNews.Add(viewModel, viewName);

                CetDataForm dataForm = usercontrol.MainDataForm as CetDataForm;

                if (usercontrol != null && dataForm != null)
                {
                    dataForm.CurrentItemChanged += dataForm_CurrentItemChanged;
                }
                usercontrol.DataContext = viewModel;
                view = region.AddView(usercontrol, viewName);
            }
            else
            {
                if (!ViewNames.ContainsKey(payLoad.Id))
                {
                    viewName = Guid.NewGuid().ToString();
                    ViewNames.Add(payLoad.Id, viewName);
                    ViewNews.Add(viewModel, viewName);
                }

                view = region.GetView(ViewNames[payLoad.Id]);

                if (view == null)
                {
                    viewModel.Load(payLoad.Id);
                    viewModel.Activate();
                    viewModel.RefreshHeader(payLoad.OpenMode);
                    usercontrol.DataContext = viewModel;

                    view = region.AddView(usercontrol, ViewNames[payLoad.Id]);
                }

            }

            region.Activate(view);
        }

        void dataForm_CurrentItemChanged(object sender, EventArgs e)
        {
            if (((CetDataForm)sender).CurrentItem == null) return;

            if (OpenMode == OpenModeType.New)
                ((CetDataForm)sender).BeginEdit();

        }
    }
}
