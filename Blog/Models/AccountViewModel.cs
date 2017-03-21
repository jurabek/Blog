using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Old password"), DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "New password"), DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
    
    public class RegisterViewModel
    {
        [Display(Name = "Name"), Required]
        public string Name { get; set; }

        [Display(Name = "Full name"), Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare(nameof(Password), ErrorMessage = "The password and confirm password does not match")]
        public string ConfirmPassword { get; set; }
    }
}