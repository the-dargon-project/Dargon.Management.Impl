using Dargon.PortableObjects;

namespace Dargon.Management.Server {
   public class LocalManagementRegistry : ILocalManagementRegistry {
      private readonly IPofSerializer pofSerializer;
      private readonly IManagementContextFactory managementContextFactory;
      private readonly ILocalManagementServerContext localManagementServerContext;
      private readonly object synchronization = new object();

      public LocalManagementRegistry(IPofSerializer pofSerializer, IManagementContextFactory managementContextFactory, ILocalManagementServerContext localManagementServerContext) {
         this.pofSerializer = pofSerializer;
         this.managementContextFactory = managementContextFactory;
         this.localManagementServerContext = localManagementServerContext;
      }

      public IManagementContext RegisterInstance(object obj) {
         lock (synchronization) {
            var context = managementContextFactory.CreateFromObject(obj);
            RegisterContext(context);
            return context;
         }
      }

      public void RegisterContext(IManagementContext managementContext) {
         lock (synchronization) {
            localManagementServerContext.HandleContextRegistered(managementContext);
         }
      }

      public void UnregisterContext(IManagementContext managementContext) {
         lock (synchronization) {
            localManagementServerContext.HandleContextUnregistered(managementContext);
         }
      }
   }
}
