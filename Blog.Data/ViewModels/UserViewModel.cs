using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Model.ViewModels
{
    public class UserViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Email confirmed")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Permissions")]
        public IEnumerable<string> Permissions { get; set; }

    }
}
