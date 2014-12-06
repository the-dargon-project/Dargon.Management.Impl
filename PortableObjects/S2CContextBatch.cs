using Dargon.PortableObjects;
using System.Collections.Generic;

namespace Dargon.Management.PortableObjects {
   public class S2CContextBatch : IPortableObject {
      private IReadOnlyList<ContextInfo> added;
      private IReadOnlyList<ContextInfo> removed;

      public S2CContextBatch() { }

      public S2CContextBatch(IReadOnlyList<ContextInfo> added, IReadOnlyList<ContextInfo> removed) {
         this.added = added;
         this.removed = removed;
      }

      public IReadOnlyList<ContextInfo> Added { get { return added; } }
      public IReadOnlyList<ContextInfo> Removed { get { return removed; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteCollection(0, added);
         writer.WriteCollection(1, removed);
      }

      public void Deserialize(IPofReader reader) {
         added = reader.ReadArray<ContextInfo>(0);
         removed = reader.ReadArray<ContextInfo>(1);
      }
   }
}
