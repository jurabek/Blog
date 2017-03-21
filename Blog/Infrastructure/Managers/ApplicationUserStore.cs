using Blog.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Infrastructure.Managers
{
    public class ApplicationUserStore : UserStore<User>
    {
        public ApplicationUserStore(BlogDbContext context) : base(context)
        {
        }
    }
}