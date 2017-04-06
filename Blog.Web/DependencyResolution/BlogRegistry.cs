// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Blog.Web.DependencyResolution
{
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.DataProtection;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
    using System;
    using System.Linq;
    using System.Web;

    public class BlogRegistry : Registry {

        public BlogRegistry() : base()
        {

            var modelAssembly = AppDomain.CurrentDomain.GetAssemblies().
                  SingleOrDefault(assembly => assembly.GetName().Name == "Blog.Model");

            var coreAssembly = AppDomain.CurrentDomain.GetAssemblies().
                  SingleOrDefault(assembly => assembly.GetName().Name == "Blog.Core");

            Scan(
                scan => {
                    scan.Assembly(modelAssembly);
                    scan.With(new ModelAssemblyConvention());
                });

            Scan(
                scan => {
                    scan.Assembly(coreAssembly);
                    scan.With(new CoreAssemblyConvention());
                });

            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
					scan.With(new ControllerConvention());
                });

#if !DEBUG_TEST
            For<IAuthenticationManager>().Use(x => HttpContext.Current.GetOwinContext().Authentication);
            For<IDataProtectionProvider>().Use(x => Startup.DataProtectionProvider);
#endif
        }
    }
}