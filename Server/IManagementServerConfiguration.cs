using ItzWarty.Networking;

namespace Dargon.Management.Server {
   public interface IManagementServerConfiguration {
      ITcpEndPoint ListeningEndpoint { get; }
   }
}
