using Blog.Abstractions.Mappings;
using Blog.Abstractions.Repositories;
using Blog.Abstractions.ViewModels;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Core.Mappings
{
    public class UserRolesMapper : IUserRolesMapper
    {
        private readonly IUserRepository<User, string> _userRepository;
        private readonly IRepository<IdentityRole, string> _roleRepository;

        public UserRolesMapper(IUserRepository<User, string> userRepository,
            IRepository<IdentityRole, string> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<IEditRoleViewModel<TRoleViewModel>> GetEditRoleViewModel<TRoleViewModel>(string userId)
            where TRoleViewModel : class, IIdentityRoleViewModel
        {
            var user = await _userRepository.GetAsync(userId);
            var userRoles = user.Roles.Select(r => r.Role);
            var model = new EditRoleViewModel
            {
                UserRoles = userRoles,
                UserId = user.Id
            };

            model.Roles = _roleRepository.GetAll().Select(r => new IdentityRoleViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Title = r.Title,
                IsSelected = user.Roles.Select(ur => ur.RoleId).Contains(r.Id)
            });

            return model as IEditRoleViewModel<TRoleViewModel>;
        }
    }
}
