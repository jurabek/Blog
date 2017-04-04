using System.Collections.Generic;

namespace Blog.Abstractions.ViewModels
{
    public interface IEditRoleViewModel<TRoleViewModel>
    {
        IEnumerable<TRoleViewModel> Roles { get; set; }
        string UserId { get; set; }
    }
}