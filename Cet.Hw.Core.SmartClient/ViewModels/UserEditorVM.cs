using Cet.Hw.Core.AppServiceContract;
using Cet.SmartClient.Client;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLocalizeExtension.Extensions;

namespace Cet.Hw.Core.SmartClient.ViewModels
{
    public class UserEditorVM : TreeNodeEditorVMBase<BaseStdData>
    {

        public UserEditorVM() : base() { }
        public UserEditorVM(IUnityContainer container) : base(container)
        { }


        #region Properties
        private UserEditorVM parentNode;
        public UserEditorVM ParentNode
        {
            get { return parentNode; }
            set { parentNode = value; }
        }

        private UserItemSearchVM userItemSearchVM;
        public UserItemSearchVM UserItemSearchVM
        {
            get { return userItemSearchVM; }
            set { userItemSearchVM = value; }
        }

        private Guid id;
        public Guid Id
        {
            get { return id; }
            private set { id = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            private set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        #endregion

        public override void PrepareSubTree()
        {
            ClearSubTree();
            var locationClient = this.Container.Resolve<IUserService>();
            foreach (var item in locationClient.FindParent(this.Id))
            {
                var editorVM = this.Container.Resolve<UserEditorVM>();
                editorVM.ParentNode = this;
                editorVM.OriginalSource = item;
                AddSubTreeNode(editorVM);
            }
            IsLoadOnDemandEnable = false;
        }

        protected override void LoadOriginalSource(BaseStdData originalSource)
        {
            this.Id = originalSource.Id;
            this.Name = originalSource.Name;
        }

        public override void PrepareChildVMs()
        {
            this.ClearChildNode();
            // Add Tab User Item 
            var userItemSearchVM = this.Container.Resolve<UserItemSearchVM>();
            userItemSearchVM.Header = new LocTextExtension() { Key = "DISPLAY_HEADER", Dict = "UserItemSearchView", Assembly = "Cet.Hw.Core.SmartClient" };
            userItemSearchVM.SearchCriteria.ParentId = this.Id;
            userItemSearchVM.Search();
            this.AddChildNode(userItemSearchVM);

            userItemSearchVM.IsSelected = true;
        }

    }
}
