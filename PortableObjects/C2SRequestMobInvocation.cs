using Dargon.PortableObjects;
using System;

namespace Dargon.Management.PortableObjects {
   public class C2SRequestMobInvocation : IPortableObject {
      private Guid mobGuid;
      private string actionName;
      private object[] parameters;

      public C2SRequestMobInvocation() { }

      public C2SRequestMobInvocation(Guid mobGuid, string actionName, object[] parameters) {
         this.mobGuid = mobGuid;
         this.actionName = actionName;
         this.parameters = parameters;
      }

      public Guid MobGuid { get { return mobGuid; } }
      public string ActionName { get { return actionName; } }
      public object[] Parameters { get { return parameters; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, mobGuid);
         writer.WriteString(1, actionName);
         writer.WriteCollection(2, parameters, true);
      }

      public void Deserialize(IPofReader reader) {
         mobGuid = reader.ReadGuid(0);
         actionName = reader.ReadString(1);
         parameters = reader.ReadArray<object>(2, true);
      }
   }
}
