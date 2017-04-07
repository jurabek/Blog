using System;
using System.Linq;
using System.Threading.Tasks;
using Blog.Abstractions.Mappings;
using Blog.Abstractions.Repositories;
using Blog.Core.Mappings;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using Moq;
using NUnit.Framework;
using Microsoft.AspNet.Identity;

namespace Blog.Web.Tests.Mappings
{
    [TestFixture]
    public class UserRolesMapperTest
    {
        private Mock<IUserRepository<User, string, IdentityResult>> _userRepository;
        private Mock<IRepository<IdentityRole, string, IdentityResult>> _roleRepository;
        private IUserRolesMapper _userRolesMapper;

        [OneTimeSetUp]
        public void Init()
        {
            _userRepository = new Mock<IUserRepository<User, string, IdentityResult>>();
            _roleRepository = new Mock<IRepository<IdentityRole, string, IdentityResult>>();
            _userRolesMapper = new UserRolesMapper(_userRepository.Object, _roleRepository.Object);
        }

        [OneTimeTearDown]
        public void Dispose()
        {
        }

        [Test]
        public async Task GetEditRoleViewModelTest()
        {
            string roleId = Guid.NewGuid().ToString("N");
            var role = new IdentityRole { Id = roleId };
            var user = new User();
            user.Roles.Add(new IdentityUserRole { RoleId = roleId, Role = role, User = user });
            user.Roles.Add(new IdentityUserRole { Role = new IdentityRole(), RoleId = "1234", User = user });

            _userRepository.Setup(x => x.GetAsync("12"))
                .Returns(Task.FromResult(user));

            _roleRepository.Setup(x => x.GetAll())
                .Returns(Enumerable.Repeat(role, 5));

            var result = await _userRolesMapper.GetEditRoleViewModel<IdentityRoleViewModel>("12");

            Assert.That(result.Roles.Count(), Is.EqualTo(5));

            Assert.That(((EditRoleViewModel)result).UserRoles.Count, Is.EqualTo(2));
        }

    }
}
