using System.Threading.Tasks;
using Blog.Abstractions.ViewModels;

namespace Blog.Abstractions.Mappings
{
    public interface IUserRolesMapper
    {
        Task<IEditRoleViewModel<TRoleViewModel>> GetEditRoleViewModel<TRoleViewModel>(string userId)
            where TRoleViewModel : class,  IIdentityRoleViewModel;
    }
}