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
        [Ignore("That was for debugging of Many to Many selecting")]
        public async Task CreateUserAndRoleTest()
        {
            var uniq = UserManager.Users.Count().ToString();
            var user = new User
            {
                Email = "test1@test.com",
                UserName = "test1"
            };

            var result = await UserManager.CreateAsync(user, "password@123");

            Assert.AreEqual(IdentityResult.Success, result);

            var role = new IdentityRole
            {
                Name = "Role 1",
                Title = "Description"
            };

            var roleResult = await RoleManager.CreateAsync(role);

            Assert.AreEqual(IdentityResult.Success, roleResult);

            await UserManager.AddToRoleAsync(user.Id, role.Name);


            var permission = await PermissionManager.CreatePermissionAsync("CanWriteTest", "User can write to test", true);
            var permission2 = await PermissionManager.CreatePermissionAsync("CanWriteTest1", "User can write to test", true);
            var permission3 = await PermissionManager.CreatePermissionAsync("CanWriteTest2", "User can write to test", true);

            await PermissionManager.AddToRole(permission, role.Id);
            await PermissionManager.AddToRole(permission2, role.Id);
            await PermissionManager.AddToRole(permission3, role.Id);



            var user2 = new User
            {
                Email = "test2@test.com",
                UserName = "test2"
            };

            var result2 = await UserManager.CreateAsync(user2, "password@123");


            var role2 = new IdentityRole
            {
                Name = "Role 2",
                Title = "Description"
            };

            var roleResult2 = await RoleManager.CreateAsync(role2);


            await UserManager.AddToRoleAsync(user2.Id, role2.Name);


            await PermissionManager.AddToRole(permission2, role2.Id);
            await PermissionManager.AddToRole(permission3, role2.Id);


            var user1Roles = Context.Roles.Where(r => r.Users.Any(u => u.UserId == user.Id)).ToList();


            var user2Roles = Context.Roles.Where(r => r.Users.Any(u => u.UserId == user2.Id)).ToList();
            

            var user1RolesPermissions = user1Roles.SelectMany(ur => ur.Permissions).Select(rp => rp.PermissionId);


            var permissions = Context.Permissions.Where(p => user1RolesPermissions.Contains(p.Id));


            var user2RolesPermissions = user2Roles.SelectMany(ur => ur.Permissions).Select(rp => rp.PermissionId);


            var permissions2 = Context.Permissions.Where(p => user2RolesPermissions.Contains(p.Id));

        }
    }
}
