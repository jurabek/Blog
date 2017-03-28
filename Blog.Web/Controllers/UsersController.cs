using AutoMapper;
using Blog.Abstractions.Fasades;
using Blog.Core;
using Blog.Core.Managers;
using Blog.Model;
using Blog.Model.ViewModels;
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
        private readonly IUserManagerFacade _userManager;
        public UsersController(IUserManagerFacade userManager)
        {
            _userManager = userManager;
        }

        // GET: Users
        public ActionResult Index()
        {
            var context = new BlogDbContext();
            IEnumerable<UserViewModel> model = Mapper.Map<IEnumerable<UserViewModel>>(context.Users.ToList());
            return View(model);
        }

        public ActionResult Edit(string id = null)
        {   
            var user = _userManager.FindByIdAsync(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(ChangeUserRolesAndPermissionsViewModel model)
        {
            return View();
        }
    }
}