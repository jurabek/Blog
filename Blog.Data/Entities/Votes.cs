using System;

namespace Blog.Model.Entities
{
    public class Vote
    {
        public int VoteValue { get; set; }
        public DateTime VotedTime { get; set; }
        public string ArticleId { get; set; }
        public virtual Article Article{ get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
