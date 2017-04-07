using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Abstractions.Facades;
using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using Moq;
using NUnit.Framework;
using Blog.Abstractions.Mappings;
using Blog.Core.Mappings;
using Blog.Model.ViewModels;
using Microsoft.AspNet.Identity;

namespace Blog.Web.Tests.Mappings
{
    [TestFixture]
    public class RolePermissionsMapperTest
    {
        private Mock<IUserRepository<User, string, IdentityResult>> _userRepository;
        private Mock<IRoleRepository<IdentityRole, string, IdentityResult>> _roleRepository;
        private Mock<IPermissionManagerFacade<IdentityPermission>> _permissionManagerFacade;
        private IRolePermissionsMapper _permissionsMapper;

        [OneTimeSetUp]
        public void Init()
        {
            _userRepository = new Mock<IUserRepository<User, string, IdentityResult>>();
            _roleRepository = new Mock<IRoleRepository<IdentityRole, string, IdentityResult>>();
            _permissionManagerFacade = new Mock<IPermissionManagerFacade<IdentityPermission>>();
            _permissionsMapper = new RolePermissionsMapper(_userRepository.Object, _roleRepository.Object, _permissionManagerFacade.Object);
        }

        [OneTimeTearDown]
        public void Dispose()
        {
        }

        [Test]
        public async Task GetEditPermissionsViewModelTest()
        {
            string roleId = Guid.NewGuid().ToString("N");
            string userId = Guid.NewGuid().ToString("N");

            var role = new IdentityRole { Id = roleId };
            var permission = new IdentityPermission();
            var rolePermission = new IdentityRolePermission
            {
                RoleId = role.Id,
                PermissionId = permission.Id
            };

            role.Permissions = new List<IdentityRolePermission> { rolePermission };

            var user = new User { Id = userId };
            user.Roles.Add(new IdentityUserRole { RoleId = role.Id, Role = role, User = user, UserId = user.Id });

            _userRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(user));

            _permissionManagerFacade.Setup(x => x.GetAll())
                .Returns(Task.FromResult(Enumerable.Repeat(permission, 6)));

            var result = await _permissionsMapper.GetEditPermissionViewModel<IdentityRole, IdentityPermissionViewModel>(userId, roleId);

            Assert.That(result.Permissions.Count(), Is.EqualTo(6));
        }
    }
}
