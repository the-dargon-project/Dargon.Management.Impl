using Dargon.PortableObjects;
using ItzWarty.Collections;
using ItzWarty.Networking;
using ItzWarty.Threading;

namespace Dargon.Management.Server {
   public class ManagementSessionFactory : IManagementSessionFactory {
      private readonly ICollectionFactory collectionFactory;
      private readonly IThreadingProxy threadingProxy;
      private readonly IPofSerializer pofSerializer;
      private readonly IMessageFactory messageFactory;

      public ManagementSessionFactory(ICollectionFactory collectionFactory, IThreadingProxy threadingProxy, IPofSerializer pofSerializer, IMessageFactory messageFactory) {
         this.collectionFactory = collectionFactory;
         this.threadingProxy = threadingProxy;
         this.pofSerializer = pofSerializer;
         this.messageFactory = messageFactory;
      }

      public IManagementSession CreateSession(IConnectedSocket socket, ILocalManagementServerContext serverContext) {
         var session = new ManagementSession(collectionFactory, threadingProxy, pofSerializer, socket, messageFactory, serverContext);
         session.Initialize();
         return session;
      }
   }
}