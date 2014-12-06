using System;
using System.Collections.Generic;
using ItzWarty.Collections;
using NLog;
using System.Linq;
using Dargon.Management.Client;
using Dargon.Management.PortableObjects;

namespace Dargon.Management.Server {
   public class LocalManagementServerContext : ILocalManagementServerContext {
      private static readonly Logger logger = LogManager.GetCurrentClassLogger();

      private readonly ICollectionFactory collectionFactory;
      private readonly IManagementSessionFactory sessionFactory;
      private readonly IHashSet<IManagementSession> sessions;
      private readonly IDictionary<Guid, IManagementContextInternal> contextsByGuid;
      private readonly object synchronization = new object();

      public LocalManagementServerContext(ICollectionFactory collectionFactory, IManagementSessionFactory sessionFactory) {
         this.collectionFactory = collectionFactory;
         this.sessionFactory = sessionFactory;
         this.sessions = collectionFactory.CreateHashSet<IManagementSession>();
         this.contextsByGuid = collectionFactory.CreateDictionary<Guid, IManagementContextInternal>();
      }

      public void HandleManagementSessionCreated(IManagementSession session) {
         lock (synchronization) {
            sessions.Add(session);
            session.SendInitialContexts(contextsByGuid.Values.ToArray(), new IManagementContext[0]);
         }
      }

      public void HandleContextRegistered(IManagementContext context) {
         lock (synchronization) {
            contextsByGuid.Add(context.Guid, (IManagementContextInternal)context);
            foreach (var session in sessions) {
               session.SendContextRegistered(context);
            }
         }
      }

      public void HandleContextUnregistered(IManagementContext context) {
         lock (synchronization) {
            contextsByGuid.Remove(context.Guid);
            foreach (var session in sessions) {
               session.SendContextUnregistered(context);
            }
         }
      }

      public IReadOnlyList<MobOperation> EnumerateMobOperations(Guid mobGuid) {
         lock (synchronization) {
            return contextsByGuid[mobGuid].EnumerateOperations();
         }
      }

      public object InvokeMobOperation(Guid mobGuid, string actionName, object[] parameters) {
         lock (synchronization) {
            return contextsByGuid[mobGuid].Invoke(actionName, parameters);
         }
      }

      public void Dispose() {
         logger.Info("Disposing management server context.");
         lock (synchronization) {
            foreach (var session in sessions) {
               session.Dispose();
            }
            contextsByGuid.Clear();
         }
      }
   }
}
