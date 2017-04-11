using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Model.Entities
{
    public class Comment
    {
        public Comment()
        {
            Id = Guid.NewGuid().ToString("N");
        }
        public string Id { get; set; }
        public string Message { get; set; }
        public string ArticleId { get; set; }
        public virtual Article Acticle { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime PublishDate { get; set; }

    }
}
