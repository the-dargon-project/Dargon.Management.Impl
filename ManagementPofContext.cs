using Dargon.Management.PortableObjects;
using Dargon.PortableObjects;

namespace Dargon.Management {
   // POF Range 1000-1999
   public class ManagementPofContext : PofContext {
      private const int kBasePofId = 1000;

      public ManagementPofContext() {
         RegisterPortableObjectType(kBasePofId + 1, typeof(S2CContextRegistered));
         RegisterPortableObjectType(kBasePofId + 2, typeof(S2CContextUnregistered));
         RegisterPortableObjectType(kBasePofId + 3, typeof(S2CContextBatch));
         RegisterPortableObjectType(kBasePofId + 4, typeof(ContextInfo));
         RegisterPortableObjectType(kBasePofId + 5, typeof(C2SRequestMobOperations));
         RegisterPortableObjectType(kBasePofId + 6, typeof(S2CMobOperations));
         RegisterPortableObjectType(kBasePofId + 7, typeof(MobOperation));
         RegisterPortableObjectType(kBasePofId + 8, typeof(MobOperationParameter));
         RegisterPortableObjectType(kBasePofId + 9, typeof(C2SRequestMobInvocation));
         RegisterPortableObjectType(kBasePofId + 10, typeof(S2CMobInvocationResult));
      }
   }
}
