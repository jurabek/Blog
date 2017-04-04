using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace IdentityPermissionExtension
{
    public interface IPermissionStore<in TKey, TPermission>
    {
        Task<TPermission> FindPermissionAsync(string name, long origin, bool isGlobal = false);
        Task<TPermission> CreatePermissionAsync(string name, long origin, string description = null,
                                                bool isGlobal = false, HttpContextBase httpContext = null);
        Task<TPermission> CreatePermissionAsync(string name, string description, bool isGlobal);
        Task DeletePermissionAsync(TKey id);
        Task<IList<string>> GetRolesAsync(string userId, ClaimsPrincipal user = null);
        Task<IList<string>> GetRolesAsync(TPermission permission);
        Task InitialConfiguration();
        Task AddToRole(TPermission permissionId, TKey roleId);
        Task RemoveFromRole(TPermission permission, TKey roleId);
        Task<IEnumerable<TPermission>> GetAll();
    }
}