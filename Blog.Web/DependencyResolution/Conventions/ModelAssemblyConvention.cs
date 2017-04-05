using StructureMap.Graph;
using System;
using System.Linq;
using StructureMap.Configuration.DSL;
using Blog.Abstractions;
using StructureMap.Pipeline;

namespace Blog.Web.DependencyResolution
{
    public class ModelAssemblyConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (!type.IsAbstract
                && type.CustomAttributes.Any()
                && type.CustomAttributes.SingleOrDefault().AttributeType == typeof(InjectableAttribute))
            {
                registry.For(type).LifecycleIs(new UniquePerRequestLifecycle());
            }   
        }
    }
}