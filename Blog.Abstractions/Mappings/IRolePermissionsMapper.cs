using System.Threading.Tasks;
using Blog.Abstractions.ViewModels;

namespace Blog.Abstractions.Mappings
{
    public interface IRolePermissionsMapper
    {
        Task<IEditPermissionViewModel<TRole, TViewModel>> GetEditPermissionViewModel<TRole, TViewModel>(string userId, string roleId)
            where TViewModel : class, new();   
    }
}