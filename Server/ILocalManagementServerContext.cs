using System;
using System.Collections.Generic;
using Dargon.Management.Client;
using Dargon.Management.PortableObjects;

namespace Dargon.Management.Server {
   public interface ILocalManagementServerContext : IDisposable {
      void HandleManagementSessionCreated(IManagementSession session);
      void HandleContextUnregistered(IManagementContext context);
      void HandleContextRegistered(IManagementContext context);
      IReadOnlyList<MobOperation> EnumerateMobOperations(Guid mobGuid);
      object InvokeMobOperation(Guid mobGuid, string actionName, object[] parameters);
   }
}
