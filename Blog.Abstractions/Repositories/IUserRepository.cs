using Blog.Abstractions.ViewModels;
using System.Threading.Tasks;

namespace Blog.Abstractions.Repositories
{
    public interface IUserRepository<T, in TKey, TResult> : IRepository<T, TKey, TResult>
        where T : class, new()
    {
        Task<TResult> AddAsync(T entity, string password);

        Task<TResult> UpdateUserRoles<TRoleViewModel>(IEditRoleViewModel<TRoleViewModel> model)
            where TRoleViewModel : IIdentityRoleViewModel;
    }
}
