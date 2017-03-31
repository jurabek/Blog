using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using Blog.Web.Controllers;
using Moq;
using NUnit.Framework;
using static Blog.Web.Controllers.ManageAccountController;

namespace Blog.Web.Tests.Controllers
{
    public class ManageAccountControllerTest 
        : BaseControllerTest<ManageAccountController, IAccountRepository<User, string>>
    {

        public override void Init()
        {
            _repository = new Mock<IAccountRepository<User, string>>();
            _controller = new ManageAccountController(_repository.Object);
        }

        [Test]
        public void IndexActionTest()
        {
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
            string errorMessage = "Old password is required";
            _controller.ModelState.AddModelError("",errorMessage);

            var result = await _controller.ChangePassword(new UpdatePasswordViewModel { OldPassword = string.Empty }) as ViewResult;
            
            Assert.AreEqual(errorMessage, result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage);

            Assert.AreEqual(string.Empty, ((UpdatePasswordViewModel)result.ViewData.Model).OldPassword);
        }

        [Test]
        public async Task ChangePasswordShouldRedirectToIndexWhenSuccess()
        {
            
        }


        [Test]
        public async Task ChangeProfileActionTest()
        {
            _repository.Setup(r => r.GetAsync(It.IsAny<string>()))
                              .Returns(Task.FromResult(new User()
                              {
                                 Name = "Test_Name",
                                 LastName = "Test_LastName"
                              }));

            var result = await _controller.ChangeProfile() as ViewResult;
            Assert.IsInstanceOf<UpdateProfileViewModel>(result.ViewData.Model);

            var model = result.ViewData.Model as UpdateProfileViewModel;

            Assert.AreEqual("Test_Name", model.Name);

            Assert.AreEqual("Test_LastName", model.LastName);
        }
        
    }
}
