using Blog.Core.Managers;
using Blog.Model;
using Blog.Model.Entities;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Web.Tests.Model
{

    //TODO:
    // That tests will be ignored it is only for Debugging... 
    [TestFixture]
    public class IdentityUserRolesAndPermissionsTest
    {
        public ApplicationUserManager UserManager { get; private set; }
        public BlogDbContext Context { get; private set; }
        public IdentityRoleStore RoleStore { get; private set; }
        public IdentityRoleManager RoleManager { get; private set; }
        public IdentityUserStore UserStore { get; private set; }
        public IdentityPermissionStore PermissionStore { get; private set; }
        public IdentityPermissionManager PermissionManager { get; private set; }

        [OneTimeSetUp]
        public void Init()
        {
            Context = new BlogDbContext();
            RoleStore = new IdentityRoleStore(Context);
            RoleManager = new IdentityRoleManager(RoleStore);
            UserStore = new IdentityUserStore(Context);
            UserManager = new ApplicationUserManager(UserStore, null);
            PermissionStore = new IdentityPermissionStore(Context);
            PermissionManager = new IdentityPermissionManager(PermissionStore);
        }


        [Test]
        public async Task CreateUserAndRoleTest()
        {
            var uniq = UserManager.Users.Count().ToString();
            var user = new User
            {
                Email = "test" + uniq + "@test.com",
                UserName = "test" + uniq
            };

            var result = await UserManager.CreateAsync(user, "password@123");

            Assert.AreEqual(IdentityResult.Success, result);

            var role = new IdentityRole
            {
                Name = "TestRole" + uniq,
                Title = "Description"
            };

            var roleResult = await RoleManager.CreateAsync(role);

            Assert.AreEqual(IdentityResult.Success, roleResult);
            

            var permission = await PermissionManager.CreatePermissionAsync("CanWriteTest_" + uniq, "User can write to test", true);

            await PermissionManager.AddToRole(permission, role.Id);

        }
    }
}
