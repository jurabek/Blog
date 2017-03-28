using Blog.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Model.ViewModels
{
    public class ChangeUserRolesAndPermissionsViewModel
    {
        public IEnumerable<IdentityPermission> AllPermissions { get; set; }

        public IEnumerable<IdentityPermission> UserPermissions { get; set; }

        public IEnumerable<IdentityRole> AllRoles { get; set; }

        public IEnumerable<IdentityRole> UserRoles { get; set; }

    }
}
