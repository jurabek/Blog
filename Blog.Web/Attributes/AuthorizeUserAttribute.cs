using Blog.Core.Managers;
using Blog.Model;
using Microsoft.AspNet.Identity;
using StructureMap;
using System.Web;
using System.Web.Mvc;
using Blog.Core.IoC;

namespace Blog.Core.Attributes
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        private IContainer _container = BaseStructuremapBootstrapper.Container;
        public Permission Permission { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string userId = httpContext.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
                return false;

            if (IsAdmin(userId) || HasPermission(userId))
                return true;

            return base.AuthorizeCore(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }

        private bool IsAdmin(string userId)
        {
            return _container.GetInstance<ApplicationUserManager>().IsInRole(userId, "Admin");
        }

        private bool HasPermission(string userId)
        {
            return false;
        }
    }
}
