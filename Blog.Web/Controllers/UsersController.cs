using AutoMapper;
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

        public UsersController(IRepository<IdentityRole, string> roleRepository, IUserRepository<User, string> userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
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
            //var user = await _userRepository.GetAsync(model.UserId);
            //var role = _roleRepository.Get(model.SelectedRoleId);

            return View();
        }

        public async Task<ActionResult> EditPermission(string userId, string roleId)
        {
            var user = await _userRepository.GetAsync(userId);

            UsersViewModel vm = Mapper.Map<UsersViewModel>(user);

            var context = new BlogDbContext();

            var userRolePermissions = vm.Roles.FirstOrDefault(r => r.Id == roleId).Permissions.Select(p => p.Permission);

            var model = new EditPermissionViewModel
            {
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

        public enum UsersMessageId
        {
            RoleAddedSuccess,
            RoleUpdateSuccess,
            Error
        }
    }
}