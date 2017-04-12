using Blog.Abstractions.Facades;
using Blog.Abstractions.Providers;
using Blog.Abstractions.Repositories;
using Blog.Core.Providers;
using Blog.Model.Entities;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Web.Tests.Providers
{
    [TestFixture]
    public class UserPermissionProviderTest
    {
        private IUserPermissionProvider _provider;
        private Mock<IUserRepository<User, string, IdentityResult>> _userRepository;
        private Mock<IPermissionManagerFacade<IdentityPermission>> _permissionFacade;


        [OneTimeSetUp]
        public void Init()
        {
            _userRepository = new Mock<IUserRepository<User, string, IdentityResult>>();
            _permissionFacade = new Mock<IPermissionManagerFacade<IdentityPermission>>();
            _provider = new UserPermissionProvider(_userRepository.Object, _permissionFacade.Object);
        }

        [Test]
        public async Task Check_permission_should_return_resutlt()
        {
            var user = new User();
            var role = new IdentityRole();
            var userRole = new IdentityUserRole()
            {
                User = user,
                Role = role
            };

            user.Roles.Add(userRole);

            _userRepository.Setup(x => x.Get(It.IsAny<string>()))
                .Returns(user);

            _permissionFacade.Setup(x => x.CheckPermission(It.IsAny<string>(), It.IsAny<IList<string>>(), true))
                .Returns(Task.FromResult(true));

            var result = await _provider.CheckUserPermission("test", "test");

            Assert.That(result, Is.EqualTo(true));
        }
    }
}
