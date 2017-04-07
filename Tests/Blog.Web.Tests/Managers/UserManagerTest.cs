using System.Threading.Tasks;
using Blog.Abstractions.Facades;
using Blog.Abstractions.Managers;
using Blog.Abstractions.Repositories;
using Blog.Abstractions.ViewModels;
using Blog.Model.Entities;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using Blog.Model.ViewModels;
using System.Linq;

namespace Blog.Web.Tests.Managers
{
    [TestFixture]
    public class UserManagerTest
    {
        private Mock<IUserManagerFacade<User>> _userManagerFacade;
        private Mock<IUserRepository<User, string, IdentityResult>> _userRepository;
        private Mock<IEmailManager> _emailManager;
        private Mock<IAuthenticationManager<User>> _authenticationManager;
        private IUserManager _userManager;

        private Mock<IMappingManager> _mappingManager;

        [OneTimeSetUp]
        public void Init()
        {
            _userManagerFacade = new Mock<IUserManagerFacade<User>>();
            _userRepository = new Mock<IUserRepository<User, string, IdentityResult>>();
            _emailManager = new Mock<IEmailManager>();
            _mappingManager = new Mock<IMappingManager>();
            _authenticationManager = new Mock<IAuthenticationManager<User>>();

            _userManager = new Blog.Core.Managers.UserManager(_userManagerFacade.Object,
                                           _userRepository.Object,
                                            _emailManager.Object,
                                            _mappingManager.Object, _authenticationManager.Object);
        }

        [Test]
        public async Task SignUpAndSignInShouldReturnSuccess()
        {
            _mappingManager.Setup(x => x.Map<IRegisterUserViewModel, User>(It.IsAny<IRegisterUserViewModel>()))
                           .Returns(new User());

            _userRepository.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<string>()))
                          .Returns(Task.FromResult(IdentityResult.Success));

            var model = new RegisterViewModel
            {
                Email = "test@test.com",
                Name = "test",
                Password = "123",
                ConfirmPassword = "123"
            };

            var result = await _userManager.SignUpAndSignIn(model);

            Assert.That(result.Succeeded, Is.EqualTo(true));
        }

        [Test]
        public async Task ResetPasswordShouldReturnErrorWhenCannotFindUserById()
        {
            _userManagerFacade.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                              .Returns(Task.FromResult<User>(null));

            var result = await _userManager.ResetPassword(new ResetPasswordViewModel());

            Assert.That(result.Errors.FirstOrDefault(),
                Is.EqualTo("We did not find user, Perhaps it was deleted or blocked, Please inform customer support!"));
        }

        [Test]
        public async Task ResetPasswordShouldSuccess()
        {
            _userManagerFacade.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                              .Returns(Task.FromResult(new User()));

            _userManagerFacade.Setup(x => x.ResetPasswordAsync(It.IsAny<string>(), "test", "123"))
                              .Returns(Task.FromResult(IdentityResult.Success));

            var result = await _userManager.ResetPassword(new ResetPasswordViewModel
            {
                Password = "123",
                ConfirmPassword = "123",
                Code = "test"
            });

            Assert.That(result.Succeeded, Is.EqualTo(true));
        }

        [Test]
        public async Task SendResetPasswordEmailShouldCreateErrorWhenCouldNotFindUserByEmail()
        {
            _userRepository.Setup(x => x.GetByNameAsync(It.IsAny<string>()))
                           .Returns(Task.FromResult<User>(null));

            var result = await _userManager.SendResetPasswordEmail(new ForgotPasswordViewModel { Email = "test@test.com" });

            Assert.That(result.Errors.FirstOrDefault(),
                                                    Is.EqualTo("The test@test.com not registired to our service!"));
        }

        [Test]
        public async Task SendResetPasswordEmailShouldSendEmail()
        {
            _userRepository.Setup(x => x.GetByNameAsync(It.IsAny<string>()))
                           .Returns(Task.FromResult(new User()));

            var result = await _userManager.SendResetPasswordEmail(new ForgotPasswordViewModel());

            Assert.That(result.Succeeded, Is.EqualTo(true));
        }

        [Test]
        public async Task ConfirmEmailShouldReturnErrorsWhenUserIdOrCodeIsEmpty()
        {
            var result = await _userManager.ConfirmEmail(null, null);
            Assert.That(result.Errors.FirstOrDefault(), Is.EqualTo("Code or UserId should not be empty"));
        }

        [Test]
        public async Task ConfirmEmailShouldSendEmailSuccessfully()
        {

            _userManagerFacade.Setup(x => x.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                              .Returns(Task.FromResult(IdentityResult.Success));

            var result = await _userManager.ConfirmEmail("test", "test");


            Assert.That(result.Succeeded, Is.EqualTo(true));
        }

        [Test]
        public async Task UpdatePasswordShouldReturnSuccess()
        {
            _userManagerFacade.Setup(x => x.ChangePassword(null, null, null))
                              .Returns(Task.FromResult(IdentityResult.Success));


            var result = await _userManager.UpdatePassword(null, new UpdatePasswordViewModel());

            Assert.That(result.Succeeded, Is.EqualTo(true));
        }

        [Test]
        public async Task UpdateProfileShouldSuccess()
        {
            _userManagerFacade.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                              .Returns(Task.FromResult(new User()));

            _userManagerFacade.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                              .Returns(Task.FromResult(IdentityResult.Success));

            var result = await _userManager.UpdateProfile("", new UpdateProfileViewModel());

            Assert.That(result.Succeeded, Is.EqualTo(true));
        }

        [Test]
        public async Task AddToRoleShouldSuccess()
        {
            _userManagerFacade.Setup(x => x.AddToRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                              .Returns(Task.FromResult(IdentityResult.Success));

            var result = await _userManager.AddToRoleAsync(null, null);

            Assert.That(result.Succeeded, Is.EqualTo(true));
        }

        [Test]
        public async Task AddToRolesShouldSuccess()
        {
            _userManagerFacade.Setup(x => x.AddToRolesAsync(null, null))
                              .Returns(Task.FromResult(IdentityResult.Success));

            var result = await _userManager.AddToRolesAsync(null, null);
            Assert.That(result.Succeeded, Is.EqualTo(true));
        }

        [Test]
        public async Task RemoveFromRoleShouldSuccess()
        {
            _userManagerFacade.Setup(x => x.RemoveFromRoleAsync(null, null))
                              .Returns(Task.FromResult(IdentityResult.Success));

            var result = await _userManager.RemoveFromRoleAsync(null, null);
            Assert.That(result.Succeeded, Is.EqualTo(true));
        }

        [Test]
        public async Task RemoveFromRolesShouldSuccess()
        {
            _userManagerFacade.Setup(x => x.RemoveFromRolesAsync(null, null))
                              .Returns(Task.FromResult(IdentityResult.Success));

            var result = await _userManager.RemoveFromRolesAsync(null, null);
            Assert.That(result.Succeeded, Is.EqualTo(true));
        }

        [Test]
        public async Task IsInRoleShouldReturnTrueWhenUserHasInRole()
        {
            _userManagerFacade.Setup(x => x.IsInRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                              .Returns(Task.FromResult(true));
            var result = await _userManager.IsInRoleAsync(null, null);

            Assert.That(result, Is.EqualTo(true));
        }
    }
}
