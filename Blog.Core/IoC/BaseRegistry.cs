using Blog.Abstractions.Fasades;
using Blog.Core.Fasades;
using Blog.Core.Managers;
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
        }
    }
}
