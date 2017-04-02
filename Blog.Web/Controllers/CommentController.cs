using Blog.Model;
using IdentityPermissionExtension;
using System.Web.Mvc;

namespace Blog.Web.Controllers
{
    public class CommentController : Controller
    {
        [AuthorizePermission(Roles = "Administrator, User", Name = nameof(Permissions.CanWriteComment))]
        public ActionResult Index()
        {
            return View();
        }
    }
}