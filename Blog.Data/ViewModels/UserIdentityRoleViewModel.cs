﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Model.ViewModels
{
    public class UserIdentityRoleViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public IList<UserIdentityPermissionViewModel> Permissions { get; set; }
    }
}
