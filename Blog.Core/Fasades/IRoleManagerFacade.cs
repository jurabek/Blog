using System.Linq;
using System.Threading.Tasks;
using Blog.Model.Entities;
using Microsoft.AspNet.Identity;

namespace Blog.Core.Fasades
{
    public interface IRoleManagerFacade
    {
        IQueryable<IdentityRole> Roles { get; }
        IdentityResult Create(IdentityRole role);
        IdentityResult Delete(IdentityRole role);
        Task<IdentityRole> FindByIdAsync(string roleId);
        Task<IdentityRole> FindByNameAsync(string roleName);
        bool RoleExists(string roleName);
        IdentityResult Update(IdentityRole role);
    }
}