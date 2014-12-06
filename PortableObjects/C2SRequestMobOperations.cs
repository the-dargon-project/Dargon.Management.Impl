using System;
using Dargon.PortableObjects;

namespace Dargon.Management.PortableObjects {
   public class C2SRequestMobOperations : IPortableObject {
      private Guid mobGuid;

      public C2SRequestMobOperations() { }

      public C2SRequestMobOperations(Guid mobGuid) {
         this.mobGuid = mobGuid;
      }

      public Guid MobGuid { get { return mobGuid; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, mobGuid);
      }

      public void Deserialize(IPofReader reader) {
         mobGuid = reader.ReadGuid(0);
      }
   }
}