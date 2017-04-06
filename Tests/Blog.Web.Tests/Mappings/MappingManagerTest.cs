using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Core.Mappings;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using NUnit.Framework;
using Blog.Abstractions.Managers;
using Blog.Core.Managers;
using Blog.Abstractions.Mappings;
using Moq;
using Blog.Abstractions.ViewModels;
using Blog.Abstractions.Repositories;

namespace Blog.Web.Tests.Mappings
{
    [TestFixture]
    public class MappingManagerTest : IDisposable
    {
        private IMappingManager _mappingManager;
        private Mock<IUserRolesMapper> _userRolesMapper;
        private Mock<IRolePermissionsMapper> _rolePermissionsMapper;

        [OneTimeSetUp]
        public void Init()
        {
            AutoMapperConfiguration.Configure();
            _userRolesMapper = new Mock<IUserRolesMapper>();
            _rolePermissionsMapper = new Mock<IRolePermissionsMapper>();
            _mappingManager = new MappingManager(_userRolesMapper.Object, _rolePermissionsMapper.Object);
        }

        [OneTimeTearDown]
        public void Dispose()
        {
        }

        [Test]
        public void ModelToViewModelWithMappingManagerTest()
        {
            var user = new User
            {
                Name = "Map Name",
                LastName = "Map LastName"
            };

            var vm = _mappingManager.Map<User, UpdateProfileViewModel>(user);

            Assert.AreEqual("Map Name", vm.Name);
            Assert.AreEqual("Map LastName", vm.LastName);
        }

        [Test]
        public void GetRolePermissionsMapperShouldNotReturnNull()
        {
            var result = _mappingManager.GetRolePermissionsMapper();

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetUserRolesMapperShouldNotReturnNull()
        {
            var result = _mappingManager.GetUserRolesMapper();

            Assert.IsNotNull(result);
        }
    }
}
