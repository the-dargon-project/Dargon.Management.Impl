using Dargon.PortableObjects;

namespace Dargon.Management.PortableObjects {
   public class S2CContextRegistered : IPortableObject {
      private ContextInfo contextInfo;

      public S2CContextRegistered() { }

      public S2CContextRegistered(ContextInfo contextInfo) {
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
