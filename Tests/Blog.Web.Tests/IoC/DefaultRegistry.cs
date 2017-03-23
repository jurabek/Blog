using Blog.Abstractions.Fasades;
using Blog.Web.Controllers;
using Moq;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Pipeline;
using StructureMap.TypeRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Blog.Web.Tests.IoC
{
    public class ControllerConvention : IRegistrationConvention
    {
        #region Public Methods and Operators

        public void Process(Type type, Registry registry)
        {
            if (type.CanBeCastTo<Controller>() && !type.IsAbstract)
            {
                registry.For(type).LifecycleIs(new UniquePerRequestLifecycle());
            }
        }

        #endregion
    }

    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(scan =>
            {
                scan.Assembly(typeof(AccountController).Assembly);
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
                scan.With(new ControllerConvention());
            });
        }
    }
}
