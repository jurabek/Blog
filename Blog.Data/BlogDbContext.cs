using Blog.Model.Entities;
using IdentityPermissionExtension;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Blog.Model
{
    /// <summary>
    /// A DbContext class that inherited from IdentityDbContext of Permission extension.
    /// </summary>
    public class BlogDbContext 
        : IdentityDbContext<User, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityPermission,IdentityRolePermission, IdentityUserClaim>
    {

        public BlogDbContext() : base("BlogConnectionString")
        {
        }

        /// <summary>
        /// A static method which create a new instance of this DbContext
        /// </summary>
        /// <returns></returns>
        public static BlogDbContext Create()
        {
            return new BlogDbContext();
        }
    }
}