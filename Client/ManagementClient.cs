using Dargon.Management.PortableObjects;
using Dargon.Management.Utilities;
using Dargon.PortableObjects;
using ItzWarty.Collections;
using ItzWarty.Networking;
using ItzWarty.Threading;
using System;

namespace Dargon.Management.Client {
   public class ManagementClient : SessionBase, IManagementClient {
      public event RemoteManageableObjectEventHandler RemoteManageableObjectAdded;
      public event RemoteManageableObjectEventHandler RemoteManageableObjectRemoved;
      public event RemoteManagementObjectOperationsEventHandler RemoteManageableObjectOperationsResult;
      public event RemoteManagementObjectInvocationResultHandler InvocationResult;

      public ManagementClient(
         ICollectionFactory collectionFactory, 
         IThreadingProxy threadingProxy, 
         IPofSerializer pofSerializer, 
         IConnectedSocket socket
      ) : base(collectionFactory, threadingProxy, pofSerializer, socket) {
         RegisterMessageHandler<S2CContextBatch>(HandleContextBatch);
         RegisterMessageHandler<S2CContextRegistered>(HandleContextRegistered);
         RegisterMessageHandler<S2CContextUnregistered>(HandleContextUnregistered);
         RegisterMessageHandler<S2CMobOperations>(HandleMobOperations);
         RegisterMessageHandler<S2CMobInvocationResult>(HandleMobInvocationResult);
      }

      public void HandleContextBatch(S2CContextBatch x) {
         var capture = RemoteManageableObjectAdded;
         if (capture != null) {
            foreach (var context in x.Added) {
               capture(this, context);
            }
         }

         capture = RemoteManageableObjectRemoved;
         if (capture != null) {
            foreach (var context in x.Removed) {
               capture(this, context);
            }
         }
      }

      public void HandleContextRegistered(S2CContextRegistered x) {
         var capture = RemoteManageableObjectAdded;
         if (capture != null) {
            capture(this, x.Info);
         }
      }

      public void HandleContextUnregistered(S2CContextUnregistered x) {
         var capture = RemoteManageableObjectRemoved;
         if (capture != null) {
            capture(this, x.Info);
         }
      }

      public void RequestMobOperations(Guid mobGuid) {
         SendMessage(new C2SRequestMobOperations(mobGuid));
      }

      private void HandleMobOperations(S2CMobOperations x) {
         var capture = RemoteManageableObjectOperationsResult;
         if (capture != null) {
            capture(this, x);
         }
      }

      public void RequestInvocation(Guid mobGuid, string methodName, object[] parameters) {
         SendMessage(new C2SRequestMobInvocation(mobGuid, methodName, parameters));
      }

      private void HandleMobInvocationResult(S2CMobInvocationResult x) {
         var capture = InvocationResult;
         if (capture != null) {
            capture(this, x.Result);
         }
      }
   }
}
