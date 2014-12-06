using System.Collections.Generic;
using Dargon.Management.PortableObjects;

namespace Dargon.Management.Server {
   public class MessageFactory : IMessageFactory {
      public S2CContextRegistered ContextRegistered(ContextInfo contextInfo) {
         return new S2CContextRegistered(contextInfo);
      }

      public S2CContextUnregistered ContextUnregistered(ContextInfo contextInfo) {
         return new S2CContextUnregistered(contextInfo);
      }

      public S2CContextBatch ContextBatch(IReadOnlyList<ContextInfo> added, IReadOnlyList<ContextInfo> removed) {
         return new S2CContextBatch(added, removed);
      }
   }
}