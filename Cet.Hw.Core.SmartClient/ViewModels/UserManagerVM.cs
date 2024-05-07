using Cet.Core;
using Cet.Hw.Core.AppServiceContract;
using Cet.SmartClient.Client;
using Cet.SmartClient.Client.Resources;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cet.Hw.Core.SmartClient.ViewModels
{
    public class UserManagerVM : TreeSearchVMBase, IResourceSelector
    {
        public UserManagerVM(IUnityContainer container)
           : base(container)
        { }

        private UserEditorVM selectedItem;
        public UserEditorVM SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (!this.HasEditingChilds)
                {
                    selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                }
            }
        }
    
        public override void PrepareRootNodes()
        {
            RootNodes.Clear();
            try
            {
                var locationClient = this.Container.Resolve<IUserService>();
                var parents = locationClient.FindParent(Guid.Empty);
                foreach (BaseStdData item in parents)
                {
                    UserEditorVM editorVM = this.Container.Resolve<UserEditorVM>();
                    editorVM.OriginalSource = item;
                    RootNodes.Add(editorVM);
                }
            }
            catch(Exception ex)
            {
               if(ex is FaultException)
                {
                    var ex1 = ex  as FaultException<UserNotLoginException>;
                    if (ex1 != null)
                        MessageBox.Show(Messages.MSG_PERMISSION_DENIED, Messages.MSG_ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {

                        var ex2 = ex as FaultException<DataValidationException>;
                        if (ex2 != null)
                            MessageBox.Show(ex2.Detail.Message, Messages.MSG_WARNING_CAPTION);
                    }

                }
            }
           
        }

        #region IResourceSelector Members
        private Guid? resourceId;
        public Guid? ResourceId
        {
            get { return resourceId; }
            set
            {
                resourceId = value;
                OnPropertyChanged("ResourceId");
            }
        }
        #endregion

    }

}