using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Model.Entities
{
    public class Article
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string PictureUrl { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
