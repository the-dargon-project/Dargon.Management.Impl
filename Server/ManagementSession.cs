using Dargon.Management.PortableObjects;
using Dargon.Management.Utilities;
using Dargon.PortableObjects;
using ItzWarty.Collections;
using ItzWarty.Networking;
using ItzWarty.Threading;
using System.Collections.Generic;

namespace Dargon.Management.Server {
   public class ManagementSession : SessionBase, IManagementSession {
      private readonly IMessageFactory messageFactory;
      private readonly ILocalManagementServerContext serverContext;

      public ManagementSession(ICollectionFactory collectionFactory, IThreadingProxy threadingProxy, IPofSerializer pofSerializer, IConnectedSocket socket, IMessageFactory messageFactory, ILocalManagementServerContext serverContext) : base(collectionFactory, threadingProxy, pofSerializer, socket) {
         this.messageFactory = messageFactory;
         this.serverContext = serverContext;
         this.RegisterMessageHandler<C2SRequestMobOperations>(HandleRequestMobOperations);
         this.RegisterMessageHandler<C2SRequestMobInvocation>(HandleRequestMobInvocation);
      }

      private void HandleRequestMobOperations(C2SRequestMobOperations x) {
         var operations = serverContext.EnumerateMobOperations(x.MobGuid);
         SendMessage(new S2CMobOperations(x.MobGuid, operations));
      }

      private void HandleRequestMobInvocation(C2SRequestMobInvocation x) {
         var result = serverContext.InvokeMobOperation(x.MobGuid, x.ActionName, x.Parameters);
         SendMessage(new S2CMobInvocationResult(result));
      }

      public void SendInitialContexts(IReadOnlyList<IManagementContext> added, IReadOnlyList<IManagementContext> removed) {
         var addedInfo = ItzWarty.Util.Generate(added.Count, i => BuildContextInfo(added[i]));
         var removedInfo = ItzWarty.Util.Generate(removed.Count, i => BuildContextInfo(removed[i]));
         var message = messageFactory.ContextBatch(addedInfo, removedInfo);
         SendMessage(message);
      }

      public void SendContextRegistered(IManagementContext context) {
         var contextInfo = BuildContextInfo(context);
         var message = messageFactory.ContextRegistered(contextInfo);
         SendMessage(message);
      }

      public void SendContextUnregistered(IManagementContext context) {
         var contextInfo = BuildContextInfo(context);
         var message = messageFactory.ContextUnregistered(contextInfo);
         SendMessage(message);
      }

      private ContextInfo BuildContextInfo(IManagementContext context) {
         var contextInfo = new ContextInfo(context.Guid, context.Name);
         return contextInfo;
      }
   }
}
