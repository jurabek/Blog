using Blog.Core.Repositories;
using Blog.Model.Entities;
using Blog.Abstractions.Repositories;
using Moq;
using Blog.Abstractions.Fasades;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Linq;
using Blog.Model.ViewModels;
using Blog.Abstractions.ViewModels;
using System;
using IdentityPermissionExtension;
using System.Collections.Generic;

namespace Blog.Web.Tests.Repositories
{
    [TestFixture]
    public class RoleRepositoryTest : BaseRepositoryTest<RoleRepository, IdentityRole, string>
    {
        protected internal override IRepository<IdentityRole, string> Repository { get; set; }

        private Mock<IRoleManagerFacade<IdentityRole>> _roleManager;
        private Mock<IPermissionManagerFacade<IdentityPermission>> _permissionManager;

        public override void Init()
        {
            _roleManager = new Mock<IRoleManagerFacade<IdentityRole>>();
            _permissionManager = new Mock<IPermissionManagerFacade<IdentityPermission>>();
            Repository = new RoleRepository(_roleManager.Object, _permissionManager.Object);

            _roleManager.Setup(x => x.Create(default(IdentityRole)))
                .Returns(IdentityResult.Success);

            _roleManager.Setup(x => x.Delete(default(IdentityRole)))
                .Returns(IdentityResult.Success);

            _roleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new IdentityRole()));

            _roleManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new IdentityRole()));

            _roleManager.Setup(x => x.Update(default(IdentityRole)))
                .Returns(IdentityResult.Success);

            _roleManager.SetupGet(x => x.Roles)
                .Returns(Enumerable.Repeat(new IdentityRole(), 5).AsQueryable());
        }

        [Test]
        public async Task UpdateRolePermissionsShouldSuccess()
        {
            var repository = Repository as RoleRepository;
            string roleId = Guid.NewGuid().ToString("N");
            var role = new IdentityRole { Id = roleId };

            var permission1Id = Guid.NewGuid().ToString("N");
            var permission1 = new IdentityPermission { Name = "1", Id = permission1Id };

            var permission2Id = Guid.NewGuid().ToString("N");
            var permission2 = new IdentityPermission { Name = "2", Id = permission2Id };

            var p3 = new IdentityPermission { Name = "3" };
            var p4 = new IdentityPermission { Name = "4" };

            var allPermissions = new List<IdentityPermission>
            {
                permission1,
                permission2,
                p3,
                p4
            };

            _permissionManager.Setup(x => x.GetAll())
                .Returns(Task.FromResult(allPermissions.AsEnumerable()));

            _roleManager.Setup(x => x.FindByIdAsync(roleId))
                .Returns(Task.FromResult(role));

            var userPermission1 = new IdentityRolePermission
            {
                Permission = permission1,
                PermissionId = permission1Id,
                Role = role,
                RoleId = roleId
            };

            var userPermission2 = new IdentityRolePermission
            {
                Permission = permission2,
                PermissionId = permission2Id,
                Role = role,
                RoleId = roleId
            };

            role.Permissions = new List<IdentityRolePermission>
            {
                userPermission1,
                userPermission2
            };            

            var permissions = new List<IdentityPermissionViewModel>
            {
                new IdentityPermissionViewModel { Id = permission1.Id, IsSelected = false },
                new IdentityPermissionViewModel { Id = permission2.Id, IsSelected = false },
                new IdentityPermissionViewModel { Id = p3.Id, IsSelected = true },
                new IdentityPermissionViewModel { Id = p3.Id, IsSelected = true }
            };

            IEditPermissionViewModel<IdentityRole, IdentityPermissionViewModel> model = new EditPermissionViewModel
            {
                Permissions = permissions,
                Role = role,
                RoleId = roleId
            };

            var result = await repository.UpdateRolePermissions<IdentityResult, IdentityPermissionViewModel>(model);

            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public async Task UpdateRoleShouldThrowException()
        {
            var result = await ((RoleRepository)Repository).UpdateRolePermissions<IdentityResult, IdentityPermissionViewModel>(null);

            Assert.IsTrue(result.Errors.Any());
        }
    }
}
