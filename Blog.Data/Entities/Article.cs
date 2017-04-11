using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Model.Entities
{
    public class Article
    {
        public Article()
        {
            Id = Guid.NewGuid().ToString("N");
        }
        public string Id { get; set; }
        public string Title { get; set; }

        [Column(TypeName = "text")]
        public string Body { get; set; }
        public string PictureUrl { get; set; }
        public DateTime PublishedDate { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
