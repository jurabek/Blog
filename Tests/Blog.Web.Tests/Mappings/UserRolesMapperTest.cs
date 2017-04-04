using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Abstractions.Mappings;
using Blog.Abstractions.Repositories;
using Blog.Core.Mappings;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using Moq;
using NUnit.Framework;

namespace Blog.Web.Tests.Mappings
{
    [TestFixture]
    public class UserRolesMapperTest
    {
        private Mock<IUserRepository<User, string>> _userRepository;
        private Mock<IRepository<IdentityRole, string>> _roleRepository;
        private IUserRolesMapper _userRolesMapper;
        [OneTimeSetUp]
        public void Init()
        {
            _userRepository = new Mock<IUserRepository<User, string>>();
            _roleRepository = new Mock<IRepository<IdentityRole, string>>();
            _userRolesMapper = new UserRolesMapper(_userRepository.Object, _roleRepository.Object);
        }

        [OneTimeTearDown]
        public void Dispose()
        {
        }

        [Test]
        public async Task GetEditRoleViewModelTest()
        {

            _userRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new User()));

            _roleRepository.Setup(x => x.GetAll())
                .Returns(Enumerable.Repeat(new IdentityRole(), 5));


            var result = await _userRolesMapper.GetEditRoleViewModel<IdentityRoleViewModel>("12");

            Assert.AreEqual(5, result.Roles.Count());
        }

    }
}
