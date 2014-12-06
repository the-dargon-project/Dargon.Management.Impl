using Dargon.Management.Client;
using Dargon.PortableObjects;

namespace Dargon.Management.PortableObjects {
   public class MobOperationParameter : IPortableObject, IRemoteMobOperationParameter {
      private string name;
      private int pofTypeId;

      public MobOperationParameter() { }

      public MobOperationParameter(string name, int pofTypeId) {
         this.name = name;
         this.pofTypeId = pofTypeId;
      }

      public string Name { get { return name; } }
      public int PofTypeId { get { return pofTypeId; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteString(0, name);
         writer.WriteS32(1, pofTypeId);
      }

      public void Deserialize(IPofReader reader) {
         name = reader.ReadString(0);
         pofTypeId = reader.ReadS32(1);
      }
   }
}