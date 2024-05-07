using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;
using System.Windows.Controls;

namespace Fs.SmartClient.Client
{
    //public class OpenEditorListener<EditorVMType, EditorViewType, KeyType, OpenEventType, OpenNewEventType>
    //    where EditorVMType : IOpenEditorVM<KeyType>
    //    where EditorViewType : UserControl
    //    where OpenEventType : CompositePresentationEvent<KeyType>, new()
    //    where OpenNewEventType : CompositePresentationEvent<object>, new()
    //{
    //    protected Dictionary<KeyType, string> ViewNames { get; set; }
    //    protected Dictionary<object, string> ViewNews { get; set; }

    //    public string ViewName { get; set; }

    //    private SubscriptionToken subscriptionTokenOpen;
    //    private SubscriptionToken subscriptionTokenOpenNew;

    //    [Dependency]
    //    public IUnityContainer Container { get; set; }

    //    [Dependency]
    //    public IEventAggregator EventAggregator { get; set; }

    //    public OpenEditorListener()
    //    {
    //        ViewNames = new Dictionary<KeyType, string>();
    //        ViewNews = new Dictionary<object, string>();
    //    }

    //    public void Initialize()
    //    {
    //        if (subscriptionTokenOpen != null)
    //        {
    //            EventAggregator.GetEvent<OpenEventType>().Unsubscribe(subscriptionTokenOpen);
    //        }
    //        subscriptionTokenOpen = EventAggregator.GetEvent<OpenEventType>().Subscribe(Open);

    //        if (subscriptionTokenOpenNew != null)
    //        {
    //            EventAggregator.GetEvent<OpenNewEventType>().Unsubscribe(subscriptionTokenOpenNew);
    //        }
    //        subscriptionTokenOpenNew = EventAggregator.GetEvent<OpenNewEventType>().Subscribe(OpenNew);
    //    }

    //    public void Open(KeyType id)
    //    {
    //        IRegionManager regionManager = Container.Resolve<IRegionManager>();
    //        IRegion region = regionManager.Regions["ContentRegion"];

    //        object view = null;
    //        if (!ViewNames.ContainsKey(id))
    //        {
    //            string viewNameUnique = Guid.NewGuid().ToString();
    //            ViewNames.Add(id, viewNameUnique);
    //        }

    //        view = region.GetView(ViewNames[id]);

    //        if (view == null)
    //        {
    //            EditorViewType usercontrol = this.Container.Resolve<EditorViewType>();

    //            EditorVMType viewModel = this.Container.Resolve<EditorVMType>();
    //            viewModel.Load(id);
    //            viewModel.Activate();
    //            viewModel.RefreshHeader(OpenModeType.Update);//viewModel.Header = string.Format(ViewName, id.ToString());
    //            usercontrol.DataContext = viewModel;

    //            view = region.AddView(usercontrol, ViewNames[id]);
    //        }
    //        else
    //        {
    //            //region.Deactivate(view);
    //        }

    //        region.Activate(view);
    //    }

    //    public virtual void OpenNew(object parameter)
    //    {
    //        IRegionManager regionManager = Container.Resolve<IRegionManager>();
    //        IRegion region = regionManager.Regions["ContentRegion"];

    //        string viewName = Guid.NewGuid().ToString();

    //        EditorViewType usercontrol = this.Container.Resolve<EditorViewType>();

    //        EditorVMType viewModel = this.Container.Resolve<EditorVMType>();
    //        viewModel.Activate();
    //        viewModel.RefreshHeader(OpenModeType.New);//viewModel.Header = string.Format(ViewName, "New");
    //        usercontrol.DataContext = viewModel;
    //        ViewNews.Add(viewModel, viewName);
    //        viewModel.EditCancelled += Cancel;

    //        object view = region.AddView(usercontrol, viewName);

    //        region.Activate(view);
    //    }

    //    protected void Cancel(object sender, EventArgs e)
    //    {
    //        if (!ViewNews.ContainsKey(sender)) return;

    //        IOpenEditorVM<KeyType> viewModel = sender as IOpenEditorVM<KeyType>;

    //        IRegionManager regionManager = Container.Resolve<IRegionManager>();
    //        IRegion region = regionManager.Regions["ContentRegion"];
    //        object view = region.GetView(ViewNews[sender]);
    //        if (view != null)
    //        {
    //            region.Remove(view);
    //            ViewNews.Remove(sender);
    //        }
    //    }
    //}

    public interface IOpenEditorVM<Key>
    {
        void Load(Key id);
        void Activate();
        event EventHandler EditCancelled;
        object Header { get; set; }
        void RefreshHeader(OpenModeType mode);
    }
}
