using Blog.Model.Entities;
using Microsoft.AspNet.Identity;

namespace Blog.Core.Managers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class IdentityRoleManager : RoleManager<IdentityRole, string>
    {
        public IdentityRoleManager(IdentityRoleStore store) : base(store)
        {
        }
    }
}
