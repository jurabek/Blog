using StructureMap.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace Blog.Web.DependencyResolution
{
    public class CoreAssemblyConvention : IRegistrationConvention
    {
        private IEnumerable<Type> abstractAssemblyInterfaces;

        public CoreAssemblyConvention()
        {
            var abstractionAssembly = AppDomain.CurrentDomain.GetAssemblies().
                  SingleOrDefault(assembly => assembly.GetName().Name == "Blog.Abstractions");

            abstractAssemblyInterfaces = abstractionAssembly.GetTypes().Where(t => t.IsInterface);
        }

        public void Process(Type type, Registry registry)
        {
            if (!type.IsAbstract)
            {
                var implementedInterface = type.GetInterfaces().Where(t => abstractAssemblyInterfaces.Select(i => i.Name).Contains(t.Name));
                if (implementedInterface.Any())
                {
                    registry.For(type.GetInterfaces().FirstOrDefault())
                        .LifecycleIs(new UniquePerRequestLifecycle())
                        .Use(type);
                }
                else
                {
                    registry.For(type).LifecycleIs(new UniquePerRequestLifecycle());
                }
            }
            
        }
    }
}