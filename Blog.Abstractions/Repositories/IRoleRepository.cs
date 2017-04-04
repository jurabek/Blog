using Blog.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Abstractions.Repositories
{
    public interface IRoleRepository<T, TKey> : IRepository<T, TKey>
        where T : class, new()
    {
        Task<TResult> UpdateRolePermissions<TResult, TViewModel>(IEditPermissionViewModel<T, TViewModel> model)
            where TResult : class
            where TViewModel : IIdentityPermissionViewModel;
    }
}
