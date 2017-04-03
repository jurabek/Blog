using AutoMapper;
using Blog.Abstractions.Managers;
using Blog.Abstractions.Repositories;
using Blog.Model;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using IdentityPermissionExtension;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Blog.Web.Controllers
{
    [Authorize(Roles = nameof(Roles.Administrator))]
    public class UsersController : Controller
    {
        private readonly IRepository<IdentityRole, string> _roleRepository;
        private IUserRepository<User, string> _userRepository;
        private IUserManager _userManager;

        public UsersController(IRepository<IdentityRole, string> roleRepository, IUserRepository<User, string> userRepository, IUserManager userManager)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            IEnumerable<UsersViewModel> model = Mapper.Map<IEnumerable<UsersViewModel>>(_userRepository.GetAll());
            return View(model);
        }

        public async Task<ActionResult> EditRole(string id = null, UsersMessageId? message = null)
        {
            var user = await _userRepository.GetAsync(id);

            UsersViewModel vm = Mapper.Map<UsersViewModel>(user);

            var context = new BlogDbContext();

            var model = new EditRoleViewModel { UserRoles = vm.Roles, UserId = vm.Id };


            var roles = context.Roles.ToList().Select(r => new IdentityRoleViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Title = r.Title,
                IsSelected = vm.Roles.Select(ur => ur.Id).Contains(r.Id)
            });

            model.Roles = roles;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditRole(EditRoleViewModel model)
        {
         

            var selectedRoles = model.Roles.Where(ir => ir.IsSelected);
            var unSelectedRoles = model.Roles.Where(ir => !ir.IsSelected);
            var context = new BlogDbContext();

            var user = await _userRepository.GetAsync(model.UserId);
            UsersViewModel vm = Mapper.Map<UsersViewModel>(user);
            var userRoles = user.Roles;

            var rolesToAdd = selectedRoles.Where(r => !userRoles.Select(i => i.RoleId).Contains(r.Id));
            var rolesToRemove = unSelectedRoles.Where(r => userRoles.Select(ur => ur.RoleId).Contains(r.Id));

            if (rolesToAdd.Any())
            {
                await _userManager.AddToRolesAsync(model.UserId, rolesToAdd.Select(ir => ir.Name).ToArray());
            }

            if (rolesToRemove.Any())
            {
                await _userManager.RemoveFromRolesAsync(model.UserId, rolesToRemove.Select(ir => ir.Name).ToArray());
            }

            model.UserRoles = vm.Roles;

            //var user = await _userRepository.GetAsync(model.UserId);
            //var role = _roleRepository.Get(model.SelectedRoleId);

            return View(model);
        }

        public async Task<ActionResult> EditPermission(string userId, string roleId)
        {
            var user = await _userRepository.GetAsync(userId);

            UsersViewModel vm = Mapper.Map<UsersViewModel>(user);

            var context = new BlogDbContext();

            var userRolePermissions = vm.Roles.FirstOrDefault(r => r.Id == roleId).Permissions.Select(p => p.Permission);

            var model = new EditPermissionViewModel
            {
                UserId = userId,
                Role = _roleRepository.Get(roleId)
            };
            
            var permissions = context.Permissions.ToList().Select(p => new IdentityPermissionViewModel
            {
                Description = p.Description,
                Id = p.Id,
                Name = p.Name,
                RoleId = roleId,
                UserId = userId,
                IsSelected = userRolePermissions.Select(rp => rp.Id).Contains(p.Id)
            });

            model.Permissions = permissions;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditPermission(EditPermissionViewModel model)
        {
            return View();
        }

        public enum UsersMessageId
        {
            RoleAddedSuccess,
            RoleUpdateSuccess,
            Error
        }
    }
}