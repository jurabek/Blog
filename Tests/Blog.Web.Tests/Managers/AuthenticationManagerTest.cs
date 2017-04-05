using System;
using System.Threading.Tasks;
using Blog.Abstractions.Facades;
using Blog.Abstractions.Managers;
using Blog.Core.Managers;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Moq;
using NUnit.Framework;

namespace Blog.Web.Tests.Managers
{
    [TestFixture]
    public class AuthenticationManagerTest
    {
        private Mock<ISignInManagerFacade<User>> _singInManagerFacade;
        private IAuthenticationManager<User> _authenticationManager;

        [OneTimeSetUp]
        public void Init()
        {
            _singInManagerFacade = new Mock<ISignInManagerFacade<User>>();
            _authenticationManager = new AuthenticationManager(_singInManagerFacade.Object);
        }

        [Test]
        public async Task SignInShouldReturnResult()
        {
            _singInManagerFacade.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                .Returns(Task.FromResult(SignInStatus.Success));

            var result = await _authenticationManager.SignIn(new LoginViewModel());

            Assert.That(result, Is.EqualTo(SignInStatus.Success));
        }

        [Test]
        public void SignOutShouldBeSuccess()
        {
            _authenticationManager.SignOut();
        }

        [Test]
        public void SignInWithUserShouldThrowException()
        {
            string errroMessage = "User should not be null";
            _singInManagerFacade.Setup(x => x.SignInAsync(null))
                                .Throws(new Exception(errroMessage));

            var ex = Assert.ThrowsAsync<Exception>(() => _authenticationManager.SignInAsync(null));

            Assert.That(ex.Message, Is.EqualTo(errroMessage));
        }

        [Test]
        public void SignWithUserShouldSuccess()
        {
            _authenticationManager.SignInAsync(new User());
        }
    }
}