using Blog.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Infrastructure
{
    public class BlogDbContext : IdentityDbContext<User>
    {
        public BlogDbContext() : base("BlogConnectionString", throwIfV1Schema: false)
        {            
        }        
    }
}