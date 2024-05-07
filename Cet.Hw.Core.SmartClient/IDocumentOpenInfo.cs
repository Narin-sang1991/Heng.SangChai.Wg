using Cet.SmartClient.Client;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Hw.Core.SmartClient
{
    public interface IDocumentOpenInfo
    {
        void OpenInfo(Guid refId, Int16 refType);
    }


    public interface IDocumentOpener : IOpenEditorVM<Guid>
    {

    }

    public class DocumentOpener : CompositePresentationEvent<GeneralOpenPayLoad<Guid, object>>
    {

    }
}
