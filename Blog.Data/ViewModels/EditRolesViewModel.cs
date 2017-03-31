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
        public string UserId { get; set; }

        public string SelectedRoleId { get; set; }

        public SelectList Roles { get; set; }

        public IList<UserIdentityPermissionViewModel> Permissions { get; set; }

        public IList<UserIdentityRoleViewModel> UserRoles { get; set; }
    }
}
