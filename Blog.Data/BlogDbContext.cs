using Blog.Abstractions;
using Blog.Model.Entities;
using IdentityPermissionExtension;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Blog.Model
{
    /// <summary>
    /// A DbContext class that inherited from IdentityDbContext of Permission extension.
    /// </summary>
    [Injectable]
    public class BlogDbContext 
        : IdentityDbContext<User, 
                            IdentityRole, 
                            string, 
                            IdentityUserLogin, 
                            IdentityUserRole, 
                            IdentityPermission,
                            IdentityRolePermission, IdentityUserClaim>
    {

        public BlogDbContext() : base("BlogConnectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<Vote> votes = modelBuilder.Entity<Vote>();
            var articles = modelBuilder.Entity<Article>();

            votes.HasKey(v => new
            {
                v.ArticleId,
                v.UserId
            });

            articles.HasMany(a => a.Votes)
                .WithRequired()
                .HasForeignKey(x => x.ArticleId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Vote> Votes { get; set; }

        public DbSet<Article> Articles { get; set; }

    }
}