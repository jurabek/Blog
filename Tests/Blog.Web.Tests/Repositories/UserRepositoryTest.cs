using Blog.Core.Repositories;
using Blog.Model.Entities;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using Blog.Abstractions.Repositories;
using Moq;
using Blog.Abstractions.Facades;
using Microsoft.AspNet.Identity;
using Blog.Abstractions.ViewModels;
using Blog.Model.ViewModels;
using System;
using System.Collections.Generic;

namespace Blog.Web.Tests.Repositories
{
    [TestFixture]
    public class UserRepositoryTest : BaseRepositoryTest<UserRepository, User, string>
    {
        protected internal override IRepository<User, string> Repository { get; set; }
        private Mock<IUserManagerFacade<User>> _userManager;

        public override void Init()
        {
            _userManager = new Mock<IUserManagerFacade<User>>();
            Repository = new UserRepository(_userManager.Object);

            _userManager.SetupGet(x => x.Users)
                .Returns(Enumerable.Repeat(new User(), 5).AsQueryable());

            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new User()));

            _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new User()));

            _userManager.Setup(x => x.CreateAsync(default(User)))
                .Returns(Task.FromResult(IdentityResult.Success));

            _userManager.Setup(x => x.UpdateAsync(default(User)))
                .Returns(Task.FromResult(IdentityResult.Success));

            _userManager.Setup(x => x.DeleteAsync(default(User)))
                .Returns(Task.FromResult(IdentityResult.Success));

        }

        [Test]
        public async Task UpdateUserRolesShouldSuccess()
        {
            _userManager.Setup(x => x.AddToRolesAsync(It.IsAny<string>(), It.IsAny<string[]>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            _userManager.Setup(x => x.RemoveFromRolesAsync(It.IsAny<string>(), It.IsAny<string[]>()))
                .Returns(Task.FromResult(IdentityResult.Success));


            var repository = Repository as UserRepository;
            string userId = Guid.NewGuid().ToString("N");
            var user = new User() { Id = userId };

            _userManager.Setup(x => x.FindByIdAsync(userId))
                .Returns(Task.FromResult(user));

            string role1Id = Guid.NewGuid().ToString("N");
            var role1 = new IdentityRole { Id = role1Id, Name = "Admin" };

            string role2Id = Guid.NewGuid().ToString("N");
            var role2 = new IdentityRole { Id = role2Id, Name = "User" };

            var role3 = new IdentityRole { Name = "Role3" };
            var role4 = new IdentityRole { Name = "Role4" };
            var role5 = new IdentityRole { Name = "Role5" };

            var userRole1 = new IdentityUserRole
            {
                User = user,
                UserId = user.Id,
                Role = role1,
                RoleId = role1Id
            };

            var userRole2 = new IdentityUserRole
            {
                User = user,
                UserId = user.Id,
                Role = role2,
                RoleId = role2Id
            };

            user.Roles.Add(userRole1);
            user.Roles.Add(userRole2);

            var roles = new List<IdentityRoleViewModel>
            {
                new IdentityRoleViewModel { IsSelected = true, Id = role3.Id, Name = role3.Name },
                new IdentityRoleViewModel { IsSelected = true, Id = role4.Id, Name = role3.Name },
                new IdentityRoleViewModel { IsSelected = true, Id = role5.Id, Name = role5.Name },
                new IdentityRoleViewModel { IsSelected = false, Name = role1.Name, Id = role1.Id },
                new IdentityRoleViewModel { IsSelected = false, Name = role2.Name, Id = role2.Id },
            };

            IEditRoleViewModel<IdentityRoleViewModel> model = new EditRoleViewModel
            {
                UserId = userId,
                Roles = roles
            };

            var result = await repository.UpdateUserRoles<IdentityResult, IdentityRoleViewModel>(model);

            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public async Task UpdateShuoldThrwoExceptionAndShouldReturnErrorMessage()
        {
            var result = await ((UserRepository)Repository).UpdateUserRoles<IdentityResult, IdentityRoleViewModel>(null);

            Assert.IsTrue(result.Errors.Any());
        }

        [Test]
        public async Task AddAsyncWithPasswordShouldSuccess()
        {
            _userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            var result = await (Repository as UserRepository).AddAsync<IdentityResult>(new User(), "123");

            Assert.IsTrue(result.Succeeded);
        }
    }
}
