using Dargon.Management.Client;
using Dargon.PortableObjects;
using System;

namespace Dargon.Management.PortableObjects {
   public class ContextInfo : IPortableObject, IRemoteMobDescription {
      private Guid guid;
      private string name;

      public ContextInfo() { }

      public ContextInfo(Guid guid, string name) {
         this.guid = guid;
         this.name = name;
      }

      public Guid Guid { get { return guid; } }
      public string Name { get { return name; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, guid);
         writer.WriteString(1, name);
      }

      public void Deserialize(IPofReader reader) {
         guid = reader.ReadGuid(0);
         name = reader.ReadString(1);
      }
   }
}
