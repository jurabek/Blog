using Blog.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Blog.Model.ViewModels
{
    public class LoginViewModel : ILoginViewModel
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
}