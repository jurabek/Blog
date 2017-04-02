using Blog.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Blog.Model.ViewModels
{
    public class RegiserViewModel : IRegisterUserViewModel
    {
        [Display(Name = "Name"), Required]
        public string Name { get; set; }

        [Display(Name = "Last name"), Required]
        public string LastName { get; set; }

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