using Dargon.PortableObjects;
using ItzWarty;
using ItzWarty.Collections;
using ItzWarty.Networking;
using ItzWarty.Threading;
using NLog;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Dargon.Management.Utilities {
   public class SessionBase {
      private static readonly Logger logger = LogManager.GetCurrentClassLogger();

      private readonly ICollectionFactory collectionFactory;
      private readonly IThreadingProxy threadingProxy;
      private readonly IPofSerializer pofSerializer;
      private readonly IConnectedSocket socket;

      private readonly IThread readerThread;
      private readonly IThread writerThread;
      private readonly IConcurrentQueue<object> outboundMessageQueue;
      private readonly ISemaphore outboundMessageReadySignal;
      private readonly ICancellationTokenSource cancellationTokenSource;
      private readonly Dictionary<Type, Action<object>> messageHandlersByMessageType;

      public SessionBase(ICollectionFactory collectionFactory, IThreadingProxy threadingProxy, IPofSerializer pofSerializer, IConnectedSocket socket) {
         this.collectionFactory = collectionFactory;
         this.threadingProxy = threadingProxy;
         this.pofSerializer = pofSerializer;
         this.socket = socket;

         this.writerThread = threadingProxy.CreateThread(WriterThreadStart, new ThreadCreationOptions() { IsBackground = true });
         this.readerThread = threadingProxy.CreateThread(ReaderThreadStart, new ThreadCreationOptions() { IsBackground = true });
         this.outboundMessageQueue = collectionFactory.CreateConcurrentQueue<object>();
         this.outboundMessageReadySignal = threadingProxy.CreateSemaphore(0, int.MaxValue);
         this.cancellationTokenSource = threadingProxy.CreateCancellationTokenSource();
         this.messageHandlersByMessageType = new Dictionary<Type, Action<object>>();
      }

      public void Initialize() {
         writerThread.Start();
         readerThread.Start();
      }

      internal void WriterThreadStart() {
         try {
            WriterThreadStartInternal();
         } catch (SocketException e) {
            logger.Warn("Writer: Caught expected socket exception " + e);
         } catch (OperationCanceledException e) {
            logger.Warn("Writer: Caught expected operation cancelled exception " + e);
         } catch (Exception e) {
            logger.Error("Writer: Caught unexpected exception " + e);
         }
      }

      private void WriterThreadStartInternal() {
         var writer = socket.GetWriter();
         while (!cancellationTokenSource.IsCancellationRequested) {
            if (outboundMessageReadySignal.Wait(cancellationTokenSource.Token)) {
               object message;
               var spinner = new SpinWait();
               while (!outboundMessageQueue.TryDequeue(out message)) {
                  spinner.SpinOnce();
               }
               pofSerializer.Serialize(writer, message);
            }
         }
      }

      internal void ReaderThreadStart() {
         try {
            ReaderThreadStartInternal();
         } catch (SocketException e) {
            logger.Warn("Reader: Caught expected socket exception " + e);
         } catch (OperationCanceledException e) {
            logger.Warn("Reader: Caught expected operation cancelled exception " + e);
         } catch (Exception e) {
            logger.Error("Reader: Caught unexpected exception " + e);
         }
      }

      private void ReaderThreadStartInternal() {
         var reader = socket.GetReader();
         while (!cancellationTokenSource.IsCancellationRequested) {
            var message = pofSerializer.Deserialize(reader.__Reader);
            var messageType = message.GetType();
            Action<object> messageHandler;
            if (messageHandlersByMessageType.TryGetValue(messageType, out messageHandler)) {
               messageHandler(message);
            } else {
               logger.Error("Reader: Unhandled message type {0}.".F(messageType.FullName));
            }
         }
      }

      protected void RegisterMessageHandler<T>(Action<T> handler) {
         messageHandlersByMessageType.Add(typeof(T), x => handler((T)x));
      }

      protected void SendMessage(IPortableObject message) {
         outboundMessageQueue.Enqueue(message);
         outboundMessageReadySignal.Release();
      }

      public void Dispose() {
         this.cancellationTokenSource.Cancel();
         this.socket.Dispose();
         this.writerThread.Join();
         this.readerThread.Join();
      }
   }
}