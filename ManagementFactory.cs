using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Management.Server;
using Dargon.PortableObjects;
using ItzWarty.Collections;
using ItzWarty.Networking;
using ItzWarty.Threading;

namespace Dargon.Management {
   public class ManagementFactoryImpl {
      private readonly ICollectionFactory collectionFactory;
      private readonly IThreadingProxy threadingProxy;
      private readonly INetworkingProxy networkingProxy;
      private readonly IPofContext pofContext;
      private readonly IPofSerializer pofSerializer;

      public ManagementFactoryImpl(ICollectionFactory collectionFactory, IThreadingProxy threadingProxy, INetworkingProxy networkingProxy, IPofContext pofContext, IPofSerializer pofSerializer) {
         this.collectionFactory = collectionFactory;
         this.threadingProxy = threadingProxy;
         this.networkingProxy = networkingProxy;
         this.pofContext = pofContext;
         this.pofSerializer = pofSerializer;
      }

      public ILocalManagementServer CreateServer(IManagementServerConfiguration configuration) {
         IMessageFactory messageFactory = new MessageFactory();
         IManagementSessionFactory managementSessionFactory = new ManagementSessionFactory(collectionFactory, threadingProxy, pofSerializer, messageFactory);
         IManagementContextFactory managementContextFactory = new ManagementContextFactory(pofContext);
         ILocalManagementServerContext localManagementServerContext = new LocalManagementServerContext(collectionFactory, managementSessionFactory);
         ILocalManagementRegistry localManagementRegistry = new LocalManagementRegistry(pofSerializer, managementContextFactory, localManagementServerContext);
         ILocalManagementListener localManagementListener = new LocalManagementListener(threadingProxy, networkingProxy, managementSessionFactory, localManagementServerContext, configuration);
         localManagementListener.Start();
         return new LocalManagementServer(localManagementRegistry, localManagementListener);
      }
   }
}
