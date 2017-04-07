using Blog.Abstractions.Mappings;
using Blog.Abstractions.Repositories;
using Blog.Abstractions.ViewModels;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System;
using Blog.Abstractions.Facades;
using Microsoft.AspNet.Identity;

namespace Blog.Core.Mappings
{
    public class RolePermissionsMapper : IRolePermissionsMapper
    {
        private IUserRepository<User, string, IdentityResult> _userRepository;
        private IRoleRepository<IdentityRole, string, IdentityResult> _roleRepository;
        private IPermissionManagerFacade<IdentityPermission> _permissionManagerFacade;

        public RolePermissionsMapper(IUserRepository<User, string, IdentityResult> userRepository,
            IRoleRepository<IdentityRole, string, IdentityResult> roleRepository,
            IPermissionManagerFacade<IdentityPermission> permissionManagerFacade)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _permissionManagerFacade = permissionManagerFacade;
        }

        public async Task<IEditPermissionViewModel<TRole, TViewModel>> GetEditPermissionViewModel<TRole, TViewModel>(string userId, string roleId) where TViewModel : class, new()
        {
            var user = await _userRepository.GetAsync(userId);
            var userRolePermissions = user.Roles.FirstOrDefault(ur => ur.RoleId == roleId).Role.Permissions;

            var model = new EditPermissionViewModel
            {
                UserId = userId,
                Role = _roleRepository.Get(roleId),
                RoleId = roleId
            };

            var allPermissions = await _permissionManagerFacade.GetAll();

            model.Permissions = allPermissions.Select(p => new IdentityPermissionViewModel
            {
                Description = p.Description,
                Id = p.Id,
                Name = p.Name,
                IsSelected = userRolePermissions.Select(rp => rp.PermissionId).Contains(p.Id)
            });

            return model as IEditPermissionViewModel<TRole, TViewModel>;
        }
        
    }
}
