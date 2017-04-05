using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Blog.Abstractions.Facades;
using Blog.Abstractions.Managers;
using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using Blog.Web.Controllers;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;

namespace Blog.Web.Tests.Controllers
{
    [TestFixture]
    public class RoleControllerTest : BaseControllerTest<RoleController>
    {
        private Mock<IRepository<IdentityRole, string>> _roleRepository;
        private Mock<IMappingManager> _mappingManager;
        private Mock<IUrlHelperFacade> _urlHelperFacade;

        public override void Init()
        {
            _roleRepository = new Mock<IRepository<IdentityRole, string>>();
            _mappingManager = new Mock<IMappingManager>();
            _urlHelperFacade = new Mock<IUrlHelperFacade>();
            _controller = new RoleController(_roleRepository.Object, _mappingManager.Object, _urlHelperFacade.Object);
        }

        [Test]
        public void IndexShouldReturnAllRoles()
        {
            _roleRepository.Setup(rp => rp.GetAll())
                .Returns(Enumerable.Repeat(new IdentityRole(), 5));

            var result = _controller.Index() as ViewResult;
            
            Assert.NotNull(result);

            
            Assert.IsNotNull(result.ViewData.Model as IEnumerable<IdentityRole>);


            Assert.AreEqual(5, ((IEnumerable<IdentityRole>) result.ViewData.Model).Count());
        }

        [Test]
        public void CreateShouldReturnView()
       {
            var result = _controller.Create();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void CreateShouldCreateModelErrors()
        {
            string errorMessage = "Role name is required";
            _controller.ModelState.AddModelError("", errorMessage);

            var result = _controller.Create(new RoleViewModel {Name = null}) as ViewResult;

            Assert.IsNotNull(result, "result != null");
            Assert.That(result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage,
                                                                                 Is.EqualTo(errorMessage));
        }

        [Test]
        public void CreateShoudRedirectToIndex()
        {
            ClearModelState();
            _roleRepository.Setup(rp => rp.Add<IdentityResult>(It.IsAny<IdentityRole>()))
                .Returns(IdentityResult.Success);

            var result = _controller.Create(new RoleViewModel()) as RedirectToRouteResult;

            Assert.NotNull(result, "result != null");
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test]
        public void CreateShouldReturnModelErrorWhenWrongDataEntered()
        {
            ClearModelState();
            string errorMessage = "Can not create role!";

            _roleRepository.Setup(rp => rp.Add<IdentityResult>(null))
                .Returns(new IdentityResult(errorMessage));

            var result = _controller.Create(null) as ViewResult;

            Assert.NotNull(result, "result != null");
            Assert.That(result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage,
                                                                                 Is.EqualTo(errorMessage));
        }

        [Test]
        public void EditShouldReturnModelToView()
        {
            ClearModelState();
            var model = new RoleViewModel();

            _roleRepository.Setup(x => x.Get(It.IsAny<string>()))
                .Returns(new IdentityRole());

            _mappingManager.Setup(x => x.Map<IdentityRole, RoleViewModel>(It.IsAny<IdentityRole>()))
                .Returns(model);

            var result = _controller.Edit("1") as ViewResult;

            Assert.IsNotNull(result);
            Assert.That(result.ViewData.Model, Is.SameAs(model)); 
        }


        [Test]
        public void EditShouldCreateModelErrors()
        {
            ClearModelState();

            string errorMessage = "Role name is required";
            _controller.ModelState.AddModelError("", errorMessage);

            var result = _controller.Edit(new RoleViewModel { Name = null }) as ViewResult;

            Assert.IsNotNull(result, "result != null");
            Assert.That(result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage,
                                                                                Is.EqualTo(errorMessage));

        }

        [Test]
        public void EditShouldRedirectToIndex()
        {
            ClearModelState();

            _roleRepository.Setup(x => x.Update<IdentityResult>(It.IsAny<IdentityRole>()))
                .Returns(IdentityResult.Success);

            var result = _controller.Edit(new RoleViewModel()) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test]
        public void DeleteShouldReturnModelToView()
        {
            var model = new IdentityRole();
            _roleRepository.Setup(x => x.Get(It.IsAny<string>()))
                .Returns(model);

            var result = _controller.Delete("1") as ViewResult;

            Assert.IsNotNull(result);

            Assert.That(result.ViewData.Model, Is.SameAs(model));
        }

        [Test]
        public void DeleteShouldRederectToIndex()
        {
            _roleRepository
                .Setup(x => x.Delete<IdentityResult>(It.IsAny<IdentityRole>()))
                .Returns(IdentityResult.Success);

            var result = _controller.Delete(new IdentityRole()) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

    }
}
