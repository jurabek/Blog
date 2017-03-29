using Blog.Abstractions.Facades;
using Blog.Abstractions.Repositories;
using Blog.Core.Fasades;
using Blog.Core.Managers;
using Blog.Core.Repositories;
using Blog.Model;
using Blog.Model.Entities;
using Microsoft.AspNet.Identity;
using StructureMap.Configuration.DSL;

namespace Blog.Core.IoC
{
    public abstract class BaseRegistry : Registry
    {
        protected BaseRegistry()
        {
            For<BlogDbContext>().Use<BlogDbContext>();
            For<IdentityRoleStore>().Use<IdentityRoleStore>();
            For<IdentityRoleManager>().Use<IdentityRoleManager>();
            For<IdentitySignInManager>().Use<IdentitySignInManager>();
            For<ApplicationUserManager>().Use<ApplicationUserManager>();
            For<ISignInManagerFacade>().Use<SignInManagerFacade>();
            For<IUserManagerFacade>().Use<UserManagerFacade>();
            For<IAccountRepository<User, string>>().Use<AccountRepository>();
            For<IUrlHelperFacade>().Use<UrlHelperFacade>();
        }
    }
}
