using System;
using System.Net.Sockets;
using ItzWarty.Networking;
using ItzWarty.Threading;
using NLog;

namespace Dargon.Management.Server {
   public class LocalManagementServer : ILocalManagementServer {
      private static readonly Logger logger = LogManager.GetCurrentClassLogger();

      private readonly IThreadingProxy threadingProxy;
      private readonly INetworkingProxy networkingProxy;
      private readonly IManagementSessionFactory managementSessionFactory;
      private readonly ILocalManagementServerContext serverContext;
      private readonly IManagementServerConfiguration configuration;

      private readonly IThread listenerThread;
      private readonly ICancellationTokenSource cancellationTokenSource;

      public LocalManagementServer(IThreadingProxy threadingProxy, INetworkingProxy networkingProxy, IManagementSessionFactory managementSessionFactory, ILocalManagementServerContext serverContext, IManagementServerConfiguration configuration) {
         this.threadingProxy = threadingProxy;
         this.networkingProxy = networkingProxy;
         this.managementSessionFactory = managementSessionFactory;
         this.serverContext = serverContext;
         this.configuration = configuration;

         this.listenerThread = threadingProxy.CreateThread(ListenerThreadStart, new ThreadCreationOptions { IsBackground = true });
         this.cancellationTokenSource = threadingProxy.CreateCancellationTokenSource();
      }

      public void Initialize() {
         listenerThread.Start();
      }

      private void ListenerThreadStart() {
         try {
            ListenerThreadStartInternal();
         } catch (SocketException e) {
            logger.Fatal("Fatal unexpected socket exception from listener thread: " + e);
         } catch (OperationCanceledException e) { 
            logger.Info("Expected cancellation exception from listener thread: " + e);
         } catch (Exception e) {
            logger.Fatal("Fatal unexpected exception from listener thread: " + e);
         }
      }

      private void ListenerThreadStartInternal() {
         var listener = networkingProxy.CreateListenerSocket(configuration.ListeningEndpoint);
         while (!cancellationTokenSource.IsCancellationRequested) {
            var client = listener.Accept(cancellationTokenSource.Token);
            var session = managementSessionFactory.CreateSession(client, serverContext);
            serverContext.HandleManagementSessionCreated(session);
         }
      }

      public void Dispose() {
         cancellationTokenSource.Cancel();
         cancellationTokenSource.Dispose();
         listenerThread.Join();
         serverContext.Dispose();
      }
   }
}
