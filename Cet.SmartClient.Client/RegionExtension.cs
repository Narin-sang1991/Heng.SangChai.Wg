using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Regions;
using Telerik.Windows.Controls;
using System.Windows.Controls;

namespace Cet.SmartClient.Client
{
    public static class RegionExtension
    {
        public static object AddView(this IRegion region, object view, string viewName)
        {
            RadDocumentPane documentPane = new RadDocumentPane();
            documentPane.ContextMenuTemplate = null;
            documentPane.Content = view;
            documentPane.DataContext = ((UserControl)view).DataContext;
            region.Add(documentPane, viewName);
            return documentPane;
        }
    }
}
