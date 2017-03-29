using Blog.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Blog.Model.ViewModels
{
    public class EditRolesViewModel
    {
        public string SelectedRoleId { get; set; }

        public SelectList Roles { get; set; }

        public IEnumerable<UserIdentityPermissionViewModel> Permissions { get; set; }

        public IEnumerable<UserIdentityRoleViewModel> UserRoles { get; set; }
    }
}
