using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Blog.Web.Controllers;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;
using Blog.Model.ViewModels;
using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using Blog.Abstractions.ViewModels;
using Microsoft.AspNet.Identity;
using Blog.Abstractions.Mappings;

namespace Blog.Web.Tests.Controllers
{
    public class AccountControllerTest 
        : BaseControllerTest<AccountController, IAccountRepository<User, string>>
    {
        private Mock<IMappingManager> _mappingManager;
        public override void Init()
        {
            _repository = new Mock<IAccountRepository<User, string>>();
            _mappingManager = new Mock<IMappingManager>();
            _controller = new AccountController(_repository.Object, _mappingManager.Object);
        }

        [Test]
        public void LoginActionTest()
        {
            ClearModelState();

            var result = _controller.Login() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
        }

        [Test]
        public async Task LoginShouldBeSuccesWhenEnteredValidViewModel()
        {
            ClearModelState();

            _repository.Setup(ar => ar.SignIn(It.IsAny<ILoginViewModel>()))
                              .Returns(Task.FromResult(SignInStatus.Success));

            var result = await _controller.Login(new LoginViewModel()) as RedirectToRouteResult;

            Assert.IsInstanceOf<ActionResult>(result);

            Assert.IsInstanceOf<RedirectToRouteResult>(result);

            Assert.AreEqual("Home",
                            result.RouteValues["controller"],
                            "Controller should be Home after logining it will rederict into Home controller");
            Assert.AreEqual("Index",
                            result.RouteValues["action"],
                            "Expected action shoulde be Index beacause if login success it rederects into Index");
        }

        [Test]
        public async Task LoginShouldBeFailWhenEnteredWrongData()
        {
            ClearModelState();

            _repository.Setup(ar => ar.SignIn(It.IsAny<ILoginViewModel>()))
                              .Returns(Task.FromResult(SignInStatus.Failure));

            var result = await _controller.Login(new LoginViewModel
            {
                Email = "fail",
                Password = "fail"
            });

            Assert.IsInstanceOf<ViewResult>(result);
            
            Assert.IsTrue(_controller.ModelState.Any(), "There is should be one error!");

            Assert.AreEqual(_controller.ModelState.Values.First().Errors.First().ErrorMessage, "Invalid login attempt.");
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
            _repository.Setup(ar => ar.SignIn(It.IsAny<ILoginViewModel>()))
                              .Returns(Task.FromResult((SignInStatus.Failure)-1));

            result = await _controller.Login(new LoginViewModel { Email = null });
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task ConfrimEmailActionTest()
        {
            ClearModelState();

            _repository.Setup(a => a.ConfirmEmail(It.IsAny<string>(), It.IsAny<string>()))
                              .Returns(Task.FromResult(IdentityResult.Success));

            var result = await _controller.ConfirmEmail(It.IsAny<string>(), It.IsAny<string>()) as ViewResult;

            Assert.AreEqual("ConfirmEmail", result.ViewName);

            _repository.Setup(a => a.ConfirmEmail("error", "error"))
                              .Returns(Task.FromResult(new IdentityResult("Error")));

            result = await _controller.ConfirmEmail("error", "error") as ViewResult;
            
            Assert.AreEqual("Error", result.ViewName);
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

            Assert.AreEqual(errorMessage, result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage);

            Assert.AreSame(model, result.ViewData.Model);
        }

        [Test]
        public async Task ForgotPasswordShouldRederictToSuccessConfirmation()
        {
            ClearModelState();

            _repository.Setup(r => r.ForgotPassword(It.IsAny<IForgotPasswordViewModel>()))
                              .Returns(Task.FromResult(IdentityResult.Success));

            var result = await _controller.ForgotPassword(new ForgotPasswordViewModel()) as RedirectToRouteResult;

            Assert.AreEqual("Account", result.RouteValues["controller"]);

            Assert.AreEqual("Success", result.RouteValues["action"]);
        }

        [Test]
        public async Task ForgotPasswordShouldReturnErrorsWhenEnteredWrongData()
        {
            ClearModelState();

            string errorMessage = "Could not sent email to user!";
            _repository.Setup(r => r.ForgotPassword(null))
                              .Returns(Task.FromResult(new IdentityResult(errorMessage)));

            var result = await _controller.ForgotPassword(null);

            Assert.AreEqual(_controller.ModelState.Values.First().Errors.First().ErrorMessage, errorMessage);
        }

        [Test]
        public void RegisterActionTest()
        {
            ClearModelState();

            var result = _controller.Register() as ViewResult;
            Assert.IsInstanceOf<ActionResult>(result);
        }

        [Test]
        public async Task RegisterModelErrorsTest()
        {
            ClearModelState();

            var model = new IRegiserViewModel();
            string errorMessage = "Validation error!";
            
            _controller.ModelState.AddModelError("validation_error", errorMessage);

            var result = await _controller.Register(model) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);

            Assert.AreEqual(errorMessage, result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage, errorMessage);
        }

        [Test]
        public async Task RegisterShouldRedirectToSuccess()
        {
            ClearModelState();

            _repository.Setup(r => r.CreateUserAsync(It.IsAny<User>(), It.IsAny<string>()))
                              .Returns(Task.FromResult(IdentityResult.Success));

            _mappingManager.Setup(r => r.Map<IRegiserViewModel,User>(It.IsAny<IRegiserViewModel>()))
                           .Returns(new User());

            var result = await _controller.Register(new IRegiserViewModel()) as RedirectToRouteResult;

            Assert.AreEqual("Success", result.RouteValues["action"]);
            Assert.AreEqual("Account", result.RouteValues["controller"]);
        }

        [Test]
        public async Task RegisterModelErrorsWhenCannotCreateUser()
        {
            _controller.ModelState.Clear();

            string errorMessage = "Can not create user!";
            _repository.Setup(r => r.CreateUserAsync(It.IsAny<User>(), It.IsAny<string>()))
                              .Returns(Task.FromResult(new IdentityResult(errorMessage)));

            var result = await _controller.Register(new IRegiserViewModel()) as ViewResult;

            Assert.AreEqual(errorMessage, result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage, errorMessage);
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

            Assert.AreEqual(errorMessage, result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage);

        }

        [Test]
        public async Task ResetPasswordShouldRedirectToSuccessAction()
        {
            ClearModelState();
            _repository.Setup(r => r.ResetPassword(It.IsAny<IResetPasswordViewModel>()))
                              .Returns(Task.FromResult(IdentityResult.Success));
            
            var result = await _controller.ResetPassword(new ResetPasswordViewModel()) as ViewResult;

            Assert.AreEqual("ResetPasswordConfirmation", result.ViewName);
        }

        [Test]
        public async Task ResetPasswordShouldReturnModelErrorWhenCanNotReset()
        {
            ClearModelState();

            string errorMessage = "Can not reset password!";
            _repository.Setup(r => r.ResetPassword(It.IsAny<IResetPasswordViewModel>()))
                              .Returns(Task.FromResult(new IdentityResult(errorMessage)));

            var result = await _controller.ResetPassword(null) as ViewResult;

            Assert.AreEqual(errorMessage, result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage);
        }

        [Test]
        public void LogOffActionTest()
        {
            var result = _controller.LogOff() as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Home", result.RouteValues["controller"]);
        }
    }
}
