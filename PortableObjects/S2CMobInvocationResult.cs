using Dargon.PortableObjects;

namespace Dargon.Management.PortableObjects {
   public class S2CMobInvocationResult : IPortableObject {
      private object result;

      public S2CMobInvocationResult() { }

      public S2CMobInvocationResult(object result) {
         this.result = result;
      }

      public object Result { get { return result; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteObject(0, result);
      }

      public void Deserialize(IPofReader reader) {
         result = reader.ReadObject(0);
      }
   }
}