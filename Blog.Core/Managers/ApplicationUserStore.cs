using Blog.Data.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Core.Managers
{
    public class ApplicationUserStore : UserStore<User>
    {
        public ApplicationUserStore(BlogDbContext context) : base(context)
        {
        }
    }
}