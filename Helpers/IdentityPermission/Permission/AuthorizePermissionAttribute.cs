using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IdentityPermissionExtension
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    /// <summary>
    /// Permission-based authorization attribute.
    /// </summary>
    public class AuthorizePermissionAttribute : AuthorizeAttribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsGlobal { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)

        {
            return Task.Run(() => httpContext.AuthorizePermission(Name, Description, IsGlobal)).Result;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
            }
        }
    }
}