using System;
using System.ComponentModel;

namespace Blog.Model
{
    [Flags]
    public enum Permission : int
    {
        [Description("Can create article")]
        CreateArticle = 1,

        [Description("Can edit artice")]
        EditAricle = 2,

        [Description("Can delete article")]
        DeleteArticle = 4,

        [Description("Can vote articles")]
        VoteArticle = 8,

        [Description("Can create comments")]
        CreateComment = 16,

        [Description("Can edit comments")]
        EditComment = 32, 

        [Description("Can delete comments")]
        DeleteComment = 64,

        [Description("All permessions")]
        All = 127
    }
}
