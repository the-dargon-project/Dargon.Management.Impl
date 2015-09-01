using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Management.Server;
using Dargon.Ryu;

namespace Dargon.Management {
   public class ManagementRyuPackage : RyuPackageV1 {
      public ManagementRyuPackage() {
         Singleton<ILocalManagementServer, LocalManagementServer>();
         Singleton<LocalManagementServer>(ryu => {
            var managementFactory = ryu.Get<ManagementFactoryImpl>();
            var managementServerConfiguration = ryu.Get<IManagementServerConfiguration>();
            return managementFactory.CreateServer(managementServerConfiguration);
         });
         Singleton<ILocalManagementRegistry, LocalManagementRegistry>();
         Singleton<LocalManagementRegistry>(ryu => ryu.Get<ILocalManagementServer>());
         PofContext<ManagementPofContext>();
      }
   }
}
