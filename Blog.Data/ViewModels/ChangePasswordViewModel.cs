using System.ComponentModel.DataAnnotations;

namespace Blog.Data.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Old password"), DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "New password"), DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}