using Blog.Abstractions.ViewModels;
using Blog.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Blog.Model.ViewModels
{
    public class EditRoleViewModel : IEditRoleViewModel<IdentityRoleViewModel>
    {
        public string UserId { get; set; }

        public IEnumerable<IdentityRole> UserRoles { get; set; }
        
        public IEnumerable<IdentityRoleViewModel> Roles { get; set; }
    }
}
