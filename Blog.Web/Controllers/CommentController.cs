using Blog.Core.Attributes;
using Blog.Model;
using IdentityPermissionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Web.Controllers
{
    public class CommentController : Controller
    {
        [AuthorizePermission(Roles = "Administrator, User", Name = "CanWriteCommentr")]
        public ActionResult Index()
        {
            return View();
        }
    }
}