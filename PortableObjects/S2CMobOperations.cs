using Dargon.Management.Client;
using Dargon.PortableObjects;
using System;
using System.Collections.Generic;

namespace Dargon.Management.PortableObjects {
   public class S2CMobOperations : IPortableObject, IRemoteMobOperationsDescription {
      private Guid guid;
      private IReadOnlyList<MobOperation> operations;

      public S2CMobOperations() { }

      public S2CMobOperations(Guid guid, IReadOnlyList<MobOperation> operations) {
         this.guid = guid;
         this.operations = operations;
      }

      public Guid Guid { get { return guid; } }
      public IReadOnlyList<MobOperation> Operations { get { return operations; } }
      IReadOnlyList<IRemoteMobOperationDescription> IRemoteMobOperationsDescription.Operations { get { return Operations; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, guid);
         writer.WriteCollection(1, operations);
      }

      public void Deserialize(IPofReader reader) {
         guid = reader.ReadGuid(0);
         operations = reader.ReadArray<MobOperation>(1);
      }
   }
}
