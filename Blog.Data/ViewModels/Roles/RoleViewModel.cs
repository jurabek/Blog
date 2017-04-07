using System.ComponentModel.DataAnnotations;

namespace Blog.Model.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Role name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Title { get; set; }

    }
}
