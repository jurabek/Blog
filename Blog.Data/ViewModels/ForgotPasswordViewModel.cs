using Blog.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Blog.Model.ViewModels
{
    public class ForgotPasswordViewModel : IForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}