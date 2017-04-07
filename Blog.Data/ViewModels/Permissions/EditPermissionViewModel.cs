using Blog.Abstractions.ViewModels;
using Blog.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Model.ViewModels
{
    public class EditPermissionViewModel : IEditPermissionViewModel<IdentityRole, IdentityPermissionViewModel>
    {
        public string UserId { get; set; }

        public string RoleId { get; set; }
        
        public IdentityRole Role { get; set; }

        public IEnumerable<IdentityPermissionViewModel> Permissions { get; set; }
    }
}
