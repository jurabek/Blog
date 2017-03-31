using AutoMapper;
using Blog.Abstractions.Facades;
using Blog.Core;
using Blog.Core.Managers;
using Blog.Model;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using IdentityPermissionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public ActionResult Index()
        {
            var context = new BlogDbContext();
            IEnumerable<UsersViewModel> model = Mapper.Map<IEnumerable<UsersViewModel>>(context.Users.ToList());
            return View(model);
        }

        public async Task<ActionResult> Edit(string id = null)
        {
            var user = await _userManager.FindByIdAsync(id) as User;
            var context = new BlogDbContext();

            var userRoles = user.Roles.Select(ur =>
                new UserIdentityRoleViewModel()
                {
                    Id = ur.RoleId,
                    Name = ur.Role.Name,
                    Title = ur.Role.Title,
                    Permissions = ur.Role.Permissions.Select(rp => new UserIdentityPermissionViewModel
                    {
                        Id = rp.PermissionId,
                        Description = rp.Permission.Description,
                        Name = rp.Permission.Name
                    }).ToList()
                });

            var model = new EditRolesViewModel
            {
                UserId = id,
                UserRoles = userRoles.ToList(),
                Roles = new SelectList(context.Roles.ToList(), "Id", "Name")
            };

            var permissons = context.Permissions.ToList();
            

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditRolesViewModel model)
        {
            return View();
        }
    }
}