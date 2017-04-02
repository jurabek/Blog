using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Model.Entities
{
    public class User : IdentityUser<string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>, IUser
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [NotMapped]
        [Display(Name = "Full name")]
        public virtual string FullName { get { return Name + " " + LastName; } }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, string> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class IdentityUserLogin : IdentityUserLogin<string>
    {
    }

    public class IdentityUserRole : IdentityUserRole<string>
    {
        public virtual User User { get; set; }

        public virtual IdentityRole Role { get; set; }
    }

    public class IdentityUserClaim : IdentityUserClaim<string>
    {
    }

    /// <summary>
    /// New models or identity models that have changed in presence of permission extension.
    /// </summary>
    public class IdentityRolePermission : IdentityPermissionExtension.IdentityRolePermission<string>
    {
        public virtual IdentityRole Role { get; set; }

        public virtual IdentityPermission Permission { get; set; }
    }

    public class IdentityPermission : IdentityPermissionExtension.IdentityPermission<string, IdentityRolePermission>
    {
        public IdentityPermission()
        {
            Id = Guid.NewGuid().ToString("N");
        }
    }

    /// <summary>
    /// The new IdentityRole should use insted of the original one.
    /// </summary>
    public class IdentityRole : IdentityPermissionExtension.IdentityRole<string, IdentityUserRole, IdentityRolePermission>
    {
        public IdentityRole()
        {
            Id = Guid.NewGuid().ToString("N");
        }
    }

    /// <summary>
    /// Note: use the permission extension DbContext in the constructor of the Stores.
    /// </summary>

    public class IdentityUserStore : UserStore<User, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public IdentityUserStore(BlogDbContext context) : base(context)
        {
        }
    }

    public class IdentityRoleStore : RoleStore<IdentityRole, string, IdentityUserRole>
    {
        public IdentityRoleStore(BlogDbContext context) : base(context)
        {
        }
    }
    
    public class IdentityPermissionStore : IdentityPermissionExtension.PermissionStore<string, IdentityRole, User, IdentityUserStore, IdentityUserLogin,
        IdentityRolePermission, IdentityUserClaim, IdentityUserRole, IdentityPermission>
    {
        private IDbSet<IdentityPermission> _permissions;
        public IdentityPermissionStore(BlogDbContext context) : base(context)
        {
            _permissions = context.Permissions;
        }

        public override Task InitialConfiguration()
        {
            //NOTE: We can enumerate enum and check it with foreach?

            if (!_permissions.Any(p => p.Name == nameof(Permissions.CanCreateArticle)))
                _permissions.Add(new IdentityPermission
                {
                    Name = nameof(Permissions.CanCreateArticle),
                    Description = "User can create articles",
                    IsGlobal = true
                });

            if (!_permissions.Any(p => p.Name == nameof(Permissions.CanEditArticle)))
                _permissions.Add(new IdentityPermission
                {
                    Name = nameof(Permissions.CanEditArticle),
                    Description = "User can edit articles",
                    IsGlobal = true
                });

            if (!_permissions.Any(p => p.Name == nameof(Permissions.CanDeleteArticle)))
                _permissions.Add(new IdentityPermission
                {
                    Name = nameof(Permissions.CanDeleteArticle),
                    Description = "User can delete articles",
                    IsGlobal = true
                });

            if (!_permissions.Any(p => p.Name == nameof(Permissions.CanVoteArticle)))
                _permissions.Add(new IdentityPermission
                {
                    Name = nameof(Permissions.CanVoteArticle),
                    Description = "User can vote articles",
                    IsGlobal = true
                });

            if (!_permissions.Any(p => p.Name == nameof(Permissions.CanWriteComment)))
                _permissions.Add(new IdentityPermission
                {
                    Name = nameof(Permissions.CanWriteComment),
                    Description = "User can write comments",
                    IsGlobal = true
                });


            if (!_permissions.Any(p => p.Name == nameof(Permissions.CanEditComment)))
                _permissions.Add(new IdentityPermission
                {
                    Name = nameof(Permissions.CanEditComment),
                    Description = "User can edit comments",
                    IsGlobal = true
                });

            if (!_permissions.Any(p => p.Name == nameof(Permissions.CanDeleteComment)))
                _permissions.Add(new IdentityPermission
                {
                    Name = nameof(Permissions.CanDeleteComment),
                    Description = "User can delete comments",
                    IsGlobal = true
                });

            Context.SaveChanges();

            return base.InitialConfiguration();
        }
    }
    
}