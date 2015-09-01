using System;
using System.Collections.Generic;
using System.Reflection;
using Dargon.Management.Client;
using Dargon.Management.PortableObjects;
using Dargon.PortableObjects;
using ItzWarty;

namespace Dargon.Management.Server {
   public class ManagementContext : IManagementContext, IManagementContextInternal {
      private readonly object obj;
      private readonly Guid guid;
      private readonly string name;
      private readonly IPofContext pofContext;
      private List<MobOperation> operations = new List<MobOperation>(); 

      public ManagementContext(object obj, Guid guid, string name, IPofContext pofContext) {
         this.obj = obj;
         this.guid = guid;
         this.name = name;
         this.pofContext = pofContext;

         var type = obj.GetType();
         foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public)) {
            var managedAttribute = method.GetCustomAttribute<ManagedOperationAttribute>();
            if (managedAttribute != null) {
               var methodParameters = method.GetParameters();
               var parameters = Util.Generate(methodParameters.Length, i => CreateMobParameter(methodParameters[i]));
               var returnTypeId = pofContext.GetTypeIdByType(method.ReturnType);
               var operation = new MobOperation(method.Name, parameters, returnTypeId);
               operations.Add(operation);
            }
         }
         foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)) {
            var managedAttribute = property.GetCustomAttribute<ManagedPropertyAttribute>();
            if (managedAttribute != null) {
               var accessors = property.GetAccessors();
               foreach (var accessor in accessors) {
                  var methodParameters = accessor.GetParameters();
                  var parameters = Util.Generate(methodParameters.Length, i => CreateMobParameter(methodParameters[i]));
                  var returnTypeId = pofContext.GetTypeIdByType(accessor.ReturnType);
                  var operation = new MobOperation(accessor.Name, parameters, returnTypeId);
                  operations.Add(operation);
               }
            }
         }
      }

      private MobOperationParameter CreateMobParameter(ParameterInfo methodParameter) {
         var typeId = pofContext.GetTypeIdByType(methodParameter.ParameterType);
         return new MobOperationParameter(methodParameter.Name, typeId);
      }

      public Guid Guid { get { return guid; } }
      public string Name { get { return name; } }
      public IReadOnlyList<MobOperation> EnumerateOperations() {
         return operations;
      }

      public object Invoke(string actionName, object[] parameters) {
         return obj.GetType().GetMethod(actionName).Invoke(obj, parameters);
      }
   }

   public interface IManagementContextInternal : IManagementContext {
      IReadOnlyList<MobOperation> EnumerateOperations();
      object Invoke(string actionName, object[] parameters);
   }
}