using Blog.Abstractions.Facades;
using Blog.Abstractions.Providers;
using Blog.Abstractions.Repositories;
using Blog.Model;
using Blog.Model.Entities;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Core.Providers
{
    public class UserPermissionProvider : IUserPermissionProvider
    {
        private IUserRepository<User, string, IdentityResult> _userRepository;
        private IPermissionManagerFacade<IdentityPermission> _permissionFacade;

        public UserPermissionProvider(
            IUserRepository<User, string, IdentityResult> userRepository,
            IPermissionManagerFacade<IdentityPermission> permissionFacade)
        {
            _userRepository = userRepository;
            _permissionFacade = permissionFacade;            
        }
        
        public async Task<bool> CheckUserPermission(string userId, string permission)
        {
            var user = _userRepository.Get(userId);
            if (user == null)
                return false;

            var roles = user.Roles.Select(r => r.Role).Select(r => r.Name).ToList();
            return await _permissionFacade.CheckPermission(permission, roles, true);
        }
    }
}
