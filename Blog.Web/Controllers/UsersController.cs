using AutoMapper;
using Blog.Abstractions.Facades;
using Blog.Abstractions.Fasades;
using Blog.Abstractions.Managers;
using Blog.Abstractions.Repositories;
using Blog.Model;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using IdentityPermissionExtension;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Blog.Web.Controllers
{
    [Authorize(Roles = nameof(Roles.Administrator))]
    public class UsersController : BaseController
    {
        private readonly IUserRepository<User, string> _userRepository;
        private IRoleRepository<IdentityRole, string> _roleRepository;
        private IMappingManager _mappingManager;

        public UsersController(IUserRepository<User, string> userRepository,
            IRoleRepository<IdentityRole, string> roleRepository, IMappingManager mappingManager, IUrlHelperFacade urlHelperFacade) : base(urlHelperFacade)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mappingManager = mappingManager;
        }

        public ActionResult Index()
        {
            IEnumerable<UsersViewModel> model = Mapper.Map<IEnumerable<UsersViewModel>>(_userRepository.GetAll());
            return View(model);
        }

        public async Task<ActionResult> EditRole(string id = null, UsersMessageId? message = null)
        {
            ViewBag.StatusMessage =
                message == UsersMessageId.RoleAddedSuccess ? "Roles has been changed." : "";

            var model = await _mappingManager.GetUserRolesMapper().GetEditRoleViewModel<IdentityRoleViewModel>(id);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditRole(EditRoleViewModel model)
        {
            var result = await _userRepository.UpdateUserRoles<IdentityResult, IdentityRoleViewModel>(model);
            if (result.Succeeded)
            {
                return RedirectToAction("EditRole", new { id = model.UserId, message = UsersMessageId.RoleAddedSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        public async Task<ActionResult> EditPermission(string userId, string roleId, UsersMessageId? message = null)
        {
            ViewBag.StatusMessage =
                message == UsersMessageId.PermissionsAddedSuccess ? "Permissions has been changed." : "";

            var model = await _mappingManager.GetRolePermissionsMapper()
                .GetEditPermissionViewModel<IdentityRole, IdentityPermissionViewModel>(userId, roleId);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditPermission(EditPermissionViewModel model)
        {
            var result = await _roleRepository.UpdateRolePermissions<IdentityResult, IdentityPermissionViewModel>(model);
            if (result.Succeeded)
            {
                return RedirectToAction("EditPermission", new { userId = model.UserId, roleId = model.RoleId, message = UsersMessageId.PermissionsAddedSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        public enum UsersMessageId
        {
            RoleAddedSuccess,
            PermissionsAddedSuccess,
            Error
        }
    }
}