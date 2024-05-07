using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls;

namespace Cet.SmartClient.Client
{
    public class TreeViewBase : UnityViewBase
    {
        protected void LoadOnDemand(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadTreeViewItem node = e.OriginalSource as RadTreeViewItem;
            if (node != null)
            {
                if (node.Item is ITreeNodeEditorVM)
                {
                    ITreeNodeEditorVM editorVM = (ITreeNodeEditorVM)node.Item;
                    editorVM.PrepareSubTree();
                }
            }
        }
    }
}
