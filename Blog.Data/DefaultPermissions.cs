using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Model
{
    public enum Permissions
    {
        CanCreateArticle,
        CanEditArticle,
        CanDeleteArticle,
        CanVoteArticle,
        CanWriteComment,
        CanEditComment,
        CanDeleteComment
    }
}
