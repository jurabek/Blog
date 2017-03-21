using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin.Security.DataProtection;
using Blog.DependencyResolution;

namespace Blog
{
    public partial class Startup
    {
        internal static IDataProtectionProvider DataProtectionProvider { get; private set; }



        public void ConfigureAuth(IAppBuilder app)
        {
            IoC.Initialize();
            DataProtectionProvider = app.GetDataProtectionProvider();
        }
    }
}