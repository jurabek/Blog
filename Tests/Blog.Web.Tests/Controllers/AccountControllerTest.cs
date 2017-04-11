using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Blog.Web.Controllers;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;
using Blog.Model.ViewModels;
using Blog.Model.Entities;
using Blog.Abstractions.ViewModels;
using Microsoft.AspNet.Identity;
using Blog.Abstractions.Managers;
using Blog.Abstractions.Facades;

namespace Blog.Web.Tests.Controllers
{
    public class AccountControllerTest
        : BaseControllerTest<AccountController>
    {
        Mock<IUserManager> _userManager;
        Mock<IAuthenticationManager<User>> _authenticationManager;
        private Mock<IUrlHelperFacade> _urlHelperFacade;

        public override void Init()
        {
            _userManager = new Mock<IUserManager>();
            _authenticationManager = new Mock<IAuthenticationManager<User>>();
            _urlHelperFacade = new Mock<IUrlHelperFacade>();
            _controller = new AccountController(_userManager.Object, _authenticationManager.Object, _urlHelperFacade.Object);
            _urlHelperFacade.Setup(uf => uf.IsLocalUrl(It.IsAny<string>()))
                .Returns(true);
        }

        [Test]
        public void When_login_then_login()
        {
            // given
            ClearModelState();

            // when
            var result = _controller.Login() as ViewResult;

            // then
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
        }

        [Test]
        public async Task Login_ShouldBe_Succes_When_Entered_Valid_Data()
        {
            ClearModelState();

            _authenticationManager.Setup(ar => ar.SignIn(It.IsAny<ILoginViewModel>()))
                              .Returns(Task.FromResult(SignInStatus.Success));

            var result = await _controller.Login(new LoginViewModel()) as RedirectToRouteResult;

            Assert.IsInstanceOf<ActionResult>(result);

            Assert.IsInstanceOf<RedirectToRouteResult>(result);

            Assert.That(result.RouteValues["controller"], Is.EqualTo("Article"));
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test]
        public async Task LoginShouldRedirectUrlWhenSuccess()
        {
            ClearModelState();

            _authenticationManager.Setup(ar => ar.SignIn(It.IsAny<ILoginViewModel>()))
                              .Returns(Task.FromResult(SignInStatus.Success));



            var result = await _controller.Login(new LoginViewModel(), "/Error/AccessDenied");

            Assert.IsInstanceOf<ActionResult>(result);
        }

        [Test]
        public async Task LoginShouldBeFailWhenEnteredWrongData()
        {
            ClearModelState();

            _authenticationManager.Setup(ar => ar.SignIn(It.IsAny<ILoginViewModel>()))
                              .Returns(Task.FromResult(SignInStatus.Failure));

            var result = await _controller.Login(new LoginViewModel
            {
                Email = "fail",
                Password = "fail"
            });

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsTrue(_controller.ModelState.Any(), "There is should be one error!");
            Assert.That(_controller.ModelState.Values.First().Errors.First().ErrorMessage, 
                                                                             Is.EqualTo("Invalid login attempt."));
        }

        [Test]
        public async Task LoginShouldReturnErrorsWhenEnteredWrongData()
        {
            ClearModelState();

            _controller.ModelState.AddModelError("error", new Exception("validation_error"));
            var result = await _controller.Login(new LoginViewModel { Email = null });
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.IsInstanceOf<ViewResult>(result);

            ClearModelState();

            // Sign In should return non of the enum value
            _authenticationManager.Setup(ar => ar.SignIn(It.IsAny<ILoginViewModel>()))
                              .Returns(Task.FromResult((SignInStatus.Failure) - 1));

            result = await _controller.Login(new LoginViewModel { Email = null });
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task ConfrimEmailActionTest()
        {
            ClearModelState();

            _userManager.Setup(a => a.ConfirmEmail(It.IsAny<string>(), It.IsAny<string>()))
                              .Returns(Task.FromResult(IdentityResult.Success));

            var result = await _controller.ConfirmEmail(It.IsAny<string>(), It.IsAny<string>()) as ViewResult;

            Assert.That(result.ViewName, Is.EqualTo("ConfirmEmail"));

            _userManager.Setup(a => a.ConfirmEmail("error", "error"))
                              .Returns(Task.FromResult(new IdentityResult("Error")));

            result = await _controller.ConfirmEmail("error", "error") as ViewResult;

            Assert.That(result.ViewName, Is.EqualTo("Error"));
        }

        [Test]
        public void ForgotPasswordActionTest()
        {
            ClearModelState();

            var forgotPasswordView = _controller.ForgotPassword() as ViewResult;
            Assert.IsInstanceOf<ViewResult>(forgotPasswordView);
        }

        [Test]
        public async Task ForgotPasswordModelErrorTest()
        {
            ClearModelState();

            var model = new ForgotPasswordViewModel();
            string errorMessage = "Email is required";
            _controller.ModelState.AddModelError("email_required", errorMessage);

            var result = await _controller.ForgotPassword(model) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);

            Assert.That(result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage, 
                                                                                 Is.EqualTo(errorMessage));

            Assert.AreSame(model, result.ViewData.Model);
        }

        [Test]
        public async Task ForgotPasswordShouldRederictToSuccessConfirmation()
        {
            ClearModelState();

            _userManager.Setup(r => r.SendResetPasswordEmail(It.IsAny<IForgotPasswordViewModel>()))
                              .Returns(Task.FromResult(IdentityResult.Success));

            var result = await _controller.ForgotPassword(new ForgotPasswordViewModel()) as RedirectToRouteResult;

            Assert.That(result.RouteValues["controller"], Is.EqualTo("Account"));

            Assert.That(result.RouteValues["action"], Is.EqualTo("Success"));
        }

        [Test]
        public async Task ForgotPasswordShouldReturnErrorsWhenEnteredWrongData()
        {
            ClearModelState();

            string errorMessage = "Could not sent email to user!";
            _userManager.Setup(r => r.SendResetPasswordEmail(null))
                              .Returns(Task.FromResult(new IdentityResult(errorMessage)));

            var result = await _controller.ForgotPassword(null);

            Assert.That(_controller.ModelState.Values.First().Errors.First().ErrorMessage, 
                                                                             Is.EqualTo(errorMessage));
        }

        [Test]
        public void RegisterActionShouldReturnViewTest()
        {
            ClearModelState();

            var result = _controller.Register() as ViewResult;
            Assert.IsInstanceOf<ActionResult>(result);
        }

        [Test]
        public async Task RegisterModelErrorsTest()
        {
            ClearModelState();

            var model = new RegisterViewModel();
            string errorMessage = "Validation error!";

            _controller.ModelState.AddModelError("validation_error", errorMessage);

            var result = await _controller.Register(model) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);

            Assert.That(result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage, 
                                                                                 Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task RegisterShouldRedirectToSuccess()
        {
            ClearModelState();

            _userManager.Setup(r => r.SignUpAndSignIn(It.IsAny<RegisterViewModel>()))
                              .Returns(Task.FromResult(IdentityResult.Success));


            var result = await _controller.Register(new RegisterViewModel()) as RedirectToRouteResult;

            Assert.That(result.RouteValues["action"], Is.EqualTo("Success"));
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Account"));
        }

        [Test]
        public async Task RegisterModelErrorsWhenCannotCreateUser()
        {
            _controller.ModelState.Clear();

            string errorMessage = "Can not create user!";
            _userManager.Setup(r => r.SignUpAndSignIn(It.IsAny<RegisterViewModel>()))
                              .Returns(Task.FromResult(new IdentityResult(errorMessage)));

            var result = await _controller.Register(new RegisterViewModel()) as ViewResult;

            Assert.That(result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage, 
                                                                                 Is.EqualTo(errorMessage));
        }

        [Test]
        public void SuccessActionTest()
        {
            ClearModelState();
            var result = _controller.Success(new SuccessViewModel()) as ViewResult;
            Assert.IsInstanceOf<SuccessViewModel>(result.ViewData.Model);
        }

        [Test]
        public void ResetPasswordActionTest()
        {
            ClearModelState();
            var result = _controller.ResetPassword(null, null) as ViewResult;
            Assert.AreEqual("Error", result.ViewName);

            result = _controller.ResetPassword("some text", "some code") as ViewResult;
            Assert.IsInstanceOf<ResetPasswordViewModel>(result.ViewData.Model);
        }

        [Test]
        public async Task ResetPasswordModelErrorsTest()
        {
            ClearModelState();

            string errorMessage = "The password and confirmation password do not match.";
            _controller.ModelState.AddModelError("", errorMessage);
            var result = await _controller.ResetPassword(new ResetPasswordViewModel
            {
                Password = "123",
                ConfirmPassword = "1234"
            }) as ViewResult;

            Assert.IsFalse(_controller.ModelState.IsValid);

            Assert.That(result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage, 
                                                                                 Is.EqualTo(errorMessage));

        }

        [Test]
        public async Task ResetPasswordShouldRedirectToSuccessAction()
        {
            ClearModelState();
            _userManager.Setup(r => r.ResetPassword(It.IsAny<IResetPasswordViewModel>()))
                              .Returns(Task.FromResult(IdentityResult.Success));

            var result = await _controller.ResetPassword(new ResetPasswordViewModel()) as ViewResult;

            Assert.That(result.ViewName, Is.EqualTo("ResetPasswordConfirmation"));
        }

        [Test]
        public async Task ResetPasswordShouldReturnModelErrorWhenCanNotReset()
        {
            ClearModelState();

            string errorMessage = "Can not reset password!";
            _userManager.Setup(r => r.ResetPassword(It.IsAny<IResetPasswordViewModel>()))
                              .Returns(Task.FromResult(new IdentityResult(errorMessage)));

            var result = await _controller.ResetPassword(null) as ViewResult;

            Assert.That(result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage, 
                                                                                 Is.EqualTo(errorMessage));
        }

        [Test]
        public void LogOffActionTest()
        {
            var result = _controller.LogOff() as RedirectToRouteResult;

            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Article"));
        }
    }
}
