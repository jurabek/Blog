using Blog.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Model.ViewModels
{
    public class EditPermissionViewModel
    {
        public IdentityRole Role { get; set; }

        public IEnumerable<IdentityPermissionViewModel> Permissions { get; set; }
    }
}
