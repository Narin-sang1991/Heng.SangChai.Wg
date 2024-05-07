using Cet.Hw.Core.SmartClient;
using Cet.SmartClient.Client;
using Microsoft.Practices.Unity;
using System;

namespace H.SangChai.WgRecords.Shell
{
    public class DocumentOpenListener : GeneralOpenEditorListenerEx<IDocumentOpener, IGeneralOpenView, DocumentOpener, Guid, object>
    {
        public override void GetParameter(IDocumentOpener editorVM, object parameter)
        {

        }
    }

    public class DocumentOpenerInfo : IDocumentOpenInfo
    {
        private IUnityContainer Container { get; set; }

        public DocumentOpenerInfo(IUnityContainer container)
        {
            this.Container = container;
        }

        public void OpenInfo(Guid refId, short refType)
        {
            throw new NotImplementedException();
        }
    }
}
