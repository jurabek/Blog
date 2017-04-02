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
        private readonly IAccountRepository<User, string> _accountRepository;

        public UsersController(IRepository<IdentityRole, string> roleRepository, IAccountRepository<User, string> accountRepository)
        {
            _roleRepository = roleRepository;
            _accountRepository = accountRepository;
        }

        public ActionResult Index()
        {
            var context = new BlogDbContext();
            IEnumerable<UsersViewModel> model = Mapper.Map<IEnumerable<UsersViewModel>>(context.Users.ToList());
            return View(model);
        }

        public async Task<ActionResult> Edit(string id = null, UsersMessageId? message = null)
        {
            var user = await _accountRepository.GetAsync(id);
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
                Roles = new SelectList(context.Roles.ToList(), "Id", "Name"),
                Permissions = new SelectList(context.Permissions.ToList(), "Id", "Description")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditRolesViewModel model)
        {
            var user = await _accountRepository.GetAsync(model.UserId);
            var role = _roleRepository.Get(model.SelectedRoleId);
            var isInRole = _accountRepository.IsInRole(user.Id, role.Name);


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