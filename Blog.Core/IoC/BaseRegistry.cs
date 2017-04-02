using Blog.Abstractions.Facades;
using Blog.Abstractions.Fasades;
using Blog.Abstractions.Managers;
using Blog.Abstractions.Repositories;
using Blog.Core.Fasades;
using Blog.Core.Managers;
using Blog.Core.Repositories;
using Blog.Model;
using Blog.Model.Entities;
using StructureMap.Configuration.DSL;

namespace Blog.Core.IoC
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public abstract class BaseRegistry : Registry
    {
        protected BaseRegistry()
        {
            For<BlogDbContext>().Use<BlogDbContext>();
            For<IdentityRoleStore>().Use<IdentityRoleStore>();
            For<IdentityRoleManager>().Use<IdentityRoleManager>();
            For<IdentitySignInManager>().Use<IdentitySignInManager>();
            For<ApplicationUserManager>().Use<ApplicationUserManager>();
            For<ISignInManagerFacade<User>>().Use<SignInManagerFacade>();
            For<IUserManagerFacade<User>>().Use<UserManagerFacade>();
            For<IUrlHelperFacade>().Use<UrlHelperFacade>();
            For<IMappingManager>().Use<MappingManager>();
            For<IRoleManagerFacade<IdentityRole>>().Use<RoleManagerFacade>();
            For<IRepository<IdentityRole, string>>().Use<RoleRepository>();
            For<IUserManager>().Use<UserManager>();
            For<IUserRepository<User, string>>().Use<UserRepository>();
            For<IEmailManager>().Use<EmailManager>();
            For<IAuthenticationManager<User>>().Use<AuthenticationManager>();
        }
    }
}
