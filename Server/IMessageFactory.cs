using System.Collections.Generic;
using Dargon.Management.PortableObjects;

namespace Dargon.Management.Server {
   public interface IMessageFactory {
      S2CContextRegistered ContextRegistered(ContextInfo contextInfo);
      S2CContextUnregistered ContextUnregistered(ContextInfo contextInfo);

      S2CContextBatch ContextBatch(IReadOnlyList<ContextInfo> added, IReadOnlyList<ContextInfo> removed);
   }
}
