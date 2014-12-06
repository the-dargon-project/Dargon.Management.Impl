using System;
using Dargon.PortableObjects;

namespace Dargon.Management.Server {
   public class ManagementContextFactory : IManagementContextFactory {
      private readonly IPofContext pofContext;

      public ManagementContextFactory(IPofContext pofContext) {
         this.pofContext = pofContext;
      }

      public IManagementContext CreateFromObject(object obj) {
         var guid = Guid.NewGuid();
         var name = obj.GetType().FullName;
         return new ManagementContext(obj, guid, name, pofContext);
      }
   }
}