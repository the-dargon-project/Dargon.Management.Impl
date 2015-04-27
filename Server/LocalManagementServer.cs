using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Management.Server {
   public class LocalManagementServer : ILocalManagementServer {
      private readonly ILocalManagementRegistry localManagementRegistry;
      private readonly ILocalManagementListener localManagementListener;

      public LocalManagementServer(ILocalManagementRegistry localManagementRegistry, ILocalManagementListener localManagementListener) {
         this.localManagementRegistry = localManagementRegistry;
         this.localManagementListener = localManagementListener;
      }

      public IManagementContext RegisterInstance(object obj) {
         return localManagementRegistry.RegisterInstance(obj);
      }

      public void RegisterContext(IManagementContext managementContext) {
         localManagementRegistry.RegisterContext(managementContext);
      }

      /// <summary>
      /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
      /// </summary>
      public void Dispose() {
         localManagementListener.Dispose();
      }
   }
}
