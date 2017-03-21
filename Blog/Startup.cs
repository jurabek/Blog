using Microsoft.Owin;
using Owin;
using System.Web;

[assembly: OwinStartup(typeof(Blog.Startup))]
namespace Blog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}