using System.Collections.Generic;
using Blog.Abstractions.ViewModels;

namespace Blog.Abstractions.ViewModels
{
    public interface IEditPermissionViewModel<TRole, TViewModel>
    {
        IEnumerable<TViewModel> Permissions { get; set; }
        TRole Role { get; set; }
        string RoleId { get; set; }
        string UserId { get; set; }
    }
}