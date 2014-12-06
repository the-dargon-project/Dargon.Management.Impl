using ItzWarty.Networking;

namespace Dargon.Management.Server {
   public interface IManagementSessionFactory {
      IManagementSession CreateSession(IConnectedSocket socket, ILocalManagementServerContext serverContext);
   }
}
