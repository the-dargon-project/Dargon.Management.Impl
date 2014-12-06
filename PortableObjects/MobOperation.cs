using Dargon.Management.Client;
using Dargon.PortableObjects;
using System.Collections.Generic;

namespace Dargon.Management.PortableObjects {
   public class MobOperation : IPortableObject, IRemoteMobOperationDescription {
      private string name;
      private IReadOnlyList<MobOperationParameter> parameters;
      private int returnPofTypeId;

      public MobOperation() { }

      public MobOperation(string name, IReadOnlyList<MobOperationParameter> parameters, int returnPofTypeId) {
         this.name = name;
         this.parameters = parameters;
         this.returnPofTypeId = returnPofTypeId;
      }

      public string Name { get { return name; } }
      public IReadOnlyList<MobOperationParameter> Parameters { get { return parameters; } }
      IReadOnlyList<IRemoteMobOperationParameter> IRemoteMobOperationDescription.Parameters { get { return Parameters; } }
      public int ReturnPofTypeId { get { return returnPofTypeId; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteString(0, name);
         writer.WriteCollection(1, parameters);
         writer.WriteS32(2, returnPofTypeId);
      }

      public void Deserialize(IPofReader reader) {
         name = reader.ReadString(0);
         parameters = reader.ReadArray<MobOperationParameter>(1);
         returnPofTypeId = reader.ReadS32(2);
      }
   }
}