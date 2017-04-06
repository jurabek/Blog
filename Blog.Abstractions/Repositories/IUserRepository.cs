using Blog.Abstractions.ViewModels;
using System.Threading.Tasks;

namespace Blog.Abstractions.Repositories
{
    public interface IUserRepository<T, in TKey> : IRepository<T, TKey>
        where T : class, new()
    {
        Task<TResult> AddAsync<TResult>(T entity, string password) where TResult : class;

        Task<TResult> UpdateUserRoles<TResult, TRoleViewModel>(IEditRoleViewModel<TRoleViewModel> model)
            where TResult : class
            where TRoleViewModel : IIdentityRoleViewModel;
    }
}
