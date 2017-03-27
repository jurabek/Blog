using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Model.ViewModels
{
    public class EditPermissionsViewModel
    {
        public Permission Permission { get; set; }

        public List<UserPermissionsModel> SelectedPermissions { get; set; }
    }

    public class UserPermissionsModel
    {
        public bool IsSelected { get; set; }
        public Permission Data { get; set; }
    }
}
