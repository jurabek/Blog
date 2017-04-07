using Blog.Model.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Model.ViewModels
{
    public class ArticleViewModel
    {
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Text")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public string ShortBody { get; set; }

        [Display(Name = "Blog image")]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        [Display(Name = "Published date")]
        public DateTime DateTime { get; set; }

        [Display(Name = "Author")]
        public User Author { get; set; }
    }
}
