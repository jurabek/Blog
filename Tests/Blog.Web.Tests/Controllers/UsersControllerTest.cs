using Blog.Abstractions.Facades;
using Blog.Abstractions.Managers;
using Blog.Abstractions.Repositories;
using Blog.Abstractions.ViewModels;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using Blog.Web.Controllers;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Blog.Web.Controllers.UsersController;

namespace Blog.Web.Tests.Controllers
{
    [TestFixture]
    public class UsersControllerTest : BaseControllerTest<UsersController>
    {
        private Mock<IUserRepository<User, string, IdentityResult>> _userRepository;
        private Mock<IRoleRepository<IdentityRole, string, IdentityResult>> _roleRepository;
        private Mock<IMappingManager> _mappingManager;
        private Mock<IUrlHelperFacade> _urlHelperFacade;

        public override void Init()
        {
            _userRepository = new Mock<IUserRepository<User, string, IdentityResult>>();
            _roleRepository = new Mock<IRoleRepository<IdentityRole, string, IdentityResult>>();
            _mappingManager = new Mock<IMappingManager>();
            _urlHelperFacade = new Mock<IUrlHelperFacade>();

            _controller = new UsersController(_userRepository.Object,
                _roleRepository.Object, 
                _mappingManager.Object, 
                _urlHelperFacade.Object);

        }

        [Test]
        public void Index_should_return_list_of_users()
        {
            var model = Enumerable.Repeat(new UsersViewModel(), 5);

            _userRepository.Setup(x => x.GetAll())
                .Returns(Enumerable.Repeat(new User(), 5));


            _mappingManager.Setup(x => x.Map<IEnumerable<User>, IEnumerable<UsersViewModel>>(It.IsAny<IEnumerable<User>>()))
                .Returns(model);

            var result = _controller.Index() as ViewResult;

            Assert.That(result.ViewData.Model, Is.EqualTo(model));
            
        }

        [Test]
        public async Task Edit_role_should_return_model()
        {
            var model = new EditRoleViewModel() as IEditRoleViewModel<IdentityRoleViewModel>;
            _mappingManager.Setup(x => x.GetUserRolesMapper()
                                        .GetEditRoleViewModel<IdentityRoleViewModel>(It.IsAny<string>()))
                                        .Returns(Task.FromResult(model));

            var result = await _controller.EditRole("test") as ViewResult;

            Assert.That(result.ViewData.Model, Is.EqualTo(model));
        }

        [Test]
        public async Task Edit_role_should_rederict_edit_role_action_whith_success_message()
        {
            var model = new EditRoleViewModel();
            _userRepository.Setup(x => x.UpdateUserRoles(model))
                .Returns(Task.FromResult(IdentityResult.Success));

            var result = await _controller.EditRole(model) as RedirectToRouteResult;

            Assert.That(result.RouteValues["action"], Is.EqualTo("EditRole"));
            Assert.That(result.RouteValues["message"], Is.EqualTo(UsersMessageId.RoleAddedSuccess));
        }

        [Test]
        public async Task Edit_permission_should_return_model()
        {
            var model = new EditPermissionViewModel() as IEditPermissionViewModel<IdentityRole, IdentityPermissionViewModel>;
            _mappingManager.Setup(x => x.GetRolePermissionsMapper()
                                    .GetEditPermissionViewModel<IdentityRole, IdentityPermissionViewModel>("", ""))
                                    .Returns(Task.FromResult(model));

            var result = await _controller.EditPermission("", "") as ViewResult;

            Assert.That(result.ViewData.Model, Is.EqualTo(model));
        }

        [Test]
        public async Task Edit_permission_should_rederict_edit_permission_action_whith_success_message()
        {
            var model = new EditPermissionViewModel();
            _roleRepository.Setup(x => x.UpdateRolePermissions(model))
                .Returns(Task.FromResult(IdentityResult.Success));

            var result = await _controller.EditPermission(model) as RedirectToRouteResult;

            Assert.That(result.RouteValues["action"], Is.EqualTo("EditPermission"));
            Assert.That(result.RouteValues["message"], Is.EqualTo(UsersMessageId.PermissionsAddedSuccess));
        }

    }
}
