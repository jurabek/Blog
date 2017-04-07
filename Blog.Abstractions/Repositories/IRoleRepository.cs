using Blog.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Abstractions.Repositories
{
    public interface IRoleRepository<T, in TKey, TResult> : IRepository<T, TKey, TResult>
        where T : class, new()
    {
        Task<TResult> UpdateRolePermissions<TViewModel>(IEditPermissionViewModel<T, TViewModel> model)
            where TViewModel : IIdentityPermissionViewModel;
    }
}
