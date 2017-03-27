using Blog.Core;
using Blog.Core.Managers;
using Blog.Model;
using IdentityPermissionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Web.Controllers
{
    [Authorize(Roles = nameof(Roles.Administrator))]
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index(IdentityRoleManager roleManager)
        {
            var context = new BlogDbContext();
        
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}