using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Blog.Abstractions.Fasades
{
    public interface IRoleManagerFacade<TRole>
    {
        IQueryable<TRole> Roles { get; }
        IdentityResult Create(TRole role);
        IdentityResult Delete(TRole role);
        Task<TRole> FindByIdAsync(string roleId);
        Task<TRole> FindByNameAsync(string roleName);
        bool RoleExists(string roleName);
        IdentityResult Update(TRole role);
    }
}