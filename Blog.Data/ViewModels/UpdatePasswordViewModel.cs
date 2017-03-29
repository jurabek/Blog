using Blog.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Blog.Model.ViewModels
{
    public class UpdatePasswordViewModel : IUpdatePasswordViewModel
    {
        [Display(Name = "Old password"), DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "New password"), DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}