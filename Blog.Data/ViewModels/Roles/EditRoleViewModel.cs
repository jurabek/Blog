using Blog.Abstractions.ViewModels;
using Blog.Model.Entities;
using System.Collections.Generic;

namespace Blog.Model.ViewModels
{
    public class EditRoleViewModel : IEditRoleViewModel<IdentityRoleViewModel>
    {
        public string UserId { get; set; }

        public IEnumerable<IdentityRole> UserRoles { get; set; }
        
        public IEnumerable<IdentityRoleViewModel> Roles { get; set; }
    }
}
