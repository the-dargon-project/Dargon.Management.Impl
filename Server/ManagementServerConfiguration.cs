using ItzWarty.Networking;

namespace Dargon.Management.Server {
   public class ManagementServerConfiguration : IManagementServerConfiguration {
      private readonly ITcpEndPoint listeningEndpoint;

      public ManagementServerConfiguration(ITcpEndPoint listeningEndpoint) {
         this.listeningEndpoint = listeningEndpoint;
      }

      public ITcpEndPoint ListeningEndpoint { get { return listeningEndpoint; } }
   }
}