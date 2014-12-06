using Dargon.PortableObjects;

namespace Dargon.Management.PortableObjects {
   public class S2CContextUnregistered : IPortableObject {
      private ContextInfo contextInfo;

      public S2CContextUnregistered() { }

      public S2CContextUnregistered(ContextInfo contextInfo) {
         this.contextInfo = contextInfo;
      }

      public ContextInfo Info { get { return contextInfo; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteObject(0, contextInfo);
      }

      public void Deserialize(IPofReader reader) {
         contextInfo = reader.ReadObject<ContextInfo>(0);
      }
   }
}
