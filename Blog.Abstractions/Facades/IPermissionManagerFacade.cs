using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Abstractions.Facades
{
    public interface IPermissionManagerFacade<TPermission>
    {
        Task AddToRole(TPermission permission, string roleId);
        Task RemoveFromRole(TPermission permission, string roleId);
        Task CreatePermissionAsync(string name, string description, bool isGlobal);
        Task DeletePermissionAsync(string id);
        Task<IEnumerable<TPermission>> GetAll();
        Task<bool> CheckPermission(string name, IList<string> roles, bool iGlobal);
    }
}