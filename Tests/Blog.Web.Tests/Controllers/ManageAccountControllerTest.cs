using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Abstractions.Mappings;
using Blog.Abstractions.Repositories;
using Blog.Abstractions.ViewModels;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using Blog.Web.Controllers;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using static Blog.Web.Controllers.ManageAccountController;

namespace Blog.Web.Tests.Controllers
{
    public class ManageAccountControllerTest 
        : BaseControllerTest<ManageAccountController, IAccountRepository<User, string>>
    {
        private Mock<IMappingManager> _mappingManager;
        public override void Init()
        {
            _repository = new Mock<IAccountRepository<User, string>>();
            _mappingManager = new Mock<IMappingManager>();
            _controller = new ManageAccountController(_repository.Object, _mappingManager.Object);
        }

        [Test]
        public void IndexActionTest()
        {
            ClearModelState();

            var result = _controller.Index(ManageMessageId.ChangePasswordSuccess) as ViewResult;

            Assert.AreEqual("Your password has been changed.", result.ViewBag.StatusMessage);

            result = _controller.Index(ManageMessageId.ChangeProfileSuccess) as ViewResult;

            Assert.AreEqual("Your profile has been changed",result.ViewBag.StatusMessage);

            result = _controller.Index(ManageMessageId.Error) as ViewResult;

            Assert.AreEqual(string.Empty, result.ViewBag.StatusMessage);
        }

        [Test]
        public void ChangePasswordActionTest()
        {
            var result = _controller.ChangePassword();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task ChangePasswordShouldReturnModelErrors()
        {
            ClearModelState();

            string errorMessage = "Old password is required";
            _controller.ModelState.AddModelError("",errorMessage);

            var result = await _controller.ChangePassword(new UpdatePasswordViewModel { OldPassword = string.Empty }) as ViewResult;
            
            Assert.AreEqual(errorMessage, result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage);

            Assert.AreEqual(string.Empty, ((UpdatePasswordViewModel)result.ViewData.Model).OldPassword);
        }

        [Test]
        public async Task ChangePasswordShouldRedirectToIndexWhenSuccess()
        {
            ClearModelState();
            _repository.Setup(r => r.UpdatePassword(It.IsAny<string>(), It.IsAny<IUpdatePasswordViewModel>()))
                       .Returns(Task.FromResult(IdentityResult.Success));

            var result = await _controller.ChangePassword(null) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);

            Assert.AreEqual(ManageMessageId.ChangePasswordSuccess , result.RouteValues["Message"]);
        }

        [Test]
        public async Task ChangePasswordShouldCreateModelErroresWhenNotSuccess()
        {
            ClearModelState();
            string errorMessage = "Can not change the password!";
            _repository.Setup(r => r.UpdatePassword(It.IsAny<string>(), It.IsAny<IUpdatePasswordViewModel>()))
                       .Returns(Task.FromResult(new IdentityResult(errorMessage)));
            
            var result = await _controller.ChangePassword(null) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);

            Assert.AreEqual(errorMessage, result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage);
        }

        [Test]
        public async Task ChangeProfileActionTest()
        {
            ClearModelState();

            string name = "Test_Name";
            string lastName = "Test_LastName";

            _repository.Setup(r => r.GetAsync(It.IsAny<string>()))
                              .Returns(Task.FromResult(new User()
                              {
                                 Name = name,
                                 LastName = lastName
                              }));

            _mappingManager.Setup(x => x.Map<User, UpdateProfileViewModel>(It.IsAny<User>()))
                            .Returns(new UpdateProfileViewModel
                            {
                                Name = name,
                                LastName = lastName
                            });

            var result = await _controller.ChangeProfile() as ViewResult;
            Assert.IsInstanceOf<UpdateProfileViewModel>(result.ViewData.Model);

            var model = result.ViewData.Model as UpdateProfileViewModel;

            Assert.AreEqual(name, model.Name);

            Assert.AreEqual(lastName, model.LastName);
        }
        

        [Test]
        public async Task ChangeProfileShouldCreateModelErrorsWhenValidationFailes()
        {
            ClearModelState();

            string errorMessage = "Name and Last name are required!";
            _controller.ModelState.AddModelError("",  errorMessage);

            var model = new UpdateProfileViewModel
            {
                Name = "",
                LastName = ""
            };

            var result = await _controller.ChangeProfile(model) as ViewResult;

            Assert.AreSame(model, result.ViewData.Model);

            Assert.AreEqual(errorMessage, result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage);
        }

        [Test]
        public async Task ChangeProfileShouldRedirectToIndexWhenSuccess()
        {
            ClearModelState();

            _repository.Setup(r => r.UpdateProfile(It.IsAny<string>(), It.IsAny<IUpdateProfileViewModel>()))
                       .Returns(Task.FromResult(IdentityResult.Success));
            
            var result = await _controller.ChangeProfile(new UpdateProfileViewModel()) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);

            Assert.AreEqual(ManageMessageId.ChangeProfileSuccess, result.RouteValues["Message"]);
        }
        
        [Test]
        public async Task ChangeProfileShouldCreateModelErrorsWhenNotSuccess()
        {
            ClearModelState();

            string errorMessage = "Can not update profile!";

            _repository.Setup(r => r.UpdateProfile(It.IsAny<string>(), It.IsAny<IUpdateProfileViewModel>()))
                       .Returns(Task.FromResult(new IdentityResult(errorMessage)));

            var result = await _controller.ChangeProfile(null) as ViewResult;

            Assert.AreEqual(errorMessage, result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage);
        } 
    }
}
