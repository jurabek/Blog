using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Managers;
using Blog.Model.Entities;
using Microsoft.AspNet.Identity;
using Blog.Abstractions.Fasades;

namespace Blog.Core.Fasades
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RoleManagerFacade : IRoleManagerFacade<IdentityRole>
    {
        private IdentityRoleManager _roleManager;

        public RoleManagerFacade(IdentityRoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// Finds role by name asynchronously
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Task<IdentityRole> FindByIdAsync(string roleId)
        {
            return _roleManager.FindByIdAsync(roleId);
        }

        /// <summary>
        /// Finds role by name
        /// </summary>
        /// <param name="roleName">Gets role name</param>
        /// <returns>IdentityRole</returns>
        public Task<IdentityRole> FindByNameAsync(string roleName)
        {
            return _roleManager.FindByNameAsync(roleName);
        }
        
        /// <summary>
        /// Gets all Roles
        /// </summary>
        /// <returns></returns>
        public IQueryable<IdentityRole> Roles
        {
            get { return _roleManager.Roles; }
        }

        public IdentityResult Create(IdentityRole role)
        {
            return _roleManager.Create(role);
        }

        public IdentityResult Delete(IdentityRole role)
        {
            return _roleManager.Delete(role);
        }

        public IdentityResult Update(IdentityRole role)
        {
            return _roleManager.Update(role);
        }

        public bool RoleExists(string roleName)
        {
            return _roleManager.RoleExists(roleName);
        }
    }
}
