using Microsoft.Owin;
using Owin;
using System.Web;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(Blog.Web.Startup))]
namespace Blog.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}