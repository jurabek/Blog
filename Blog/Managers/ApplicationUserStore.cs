using Microsoft.AspNet.Identity.EntityFramework;
using Blog.Models;

namespace Blog.Managers
{
    public class ApplicationUserStore : UserStore<User>
    {
        public ApplicationUserStore(BlogDbContext context) : base(context)
        {
        }
    }
}