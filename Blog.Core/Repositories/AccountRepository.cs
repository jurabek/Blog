using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Blog.Abstractions.Facades;
using Blog.Model.ViewModels;
using IdentityPermissionExtension;
using AutoMapper;
using Blog.Abstractions.ViewModels;
using Microsoft.AspNet.Identity.Owin;

namespace Blog.Core.Repositories
{
    public class AccountRepository : IAccountRepository<User, string>
    {
        private readonly IUserManagerFacade _userManagerFacade;
        private readonly IUrlHelperFacade _urlHelperFacade;
        private ISignInManagerFacade _signInManagerFacade;

        public AccountRepository(IUserManagerFacade userManagerFacade, ISignInManagerFacade signInManagerFacade, IUrlHelperFacade urlHelperFacade)
        {
            _urlHelperFacade = urlHelperFacade;
            _signInManagerFacade = signInManagerFacade;
            _userManagerFacade = userManagerFacade;
        }

        public async Task<IdentityResult> CreateUserAsync(IUserViewModel model)
        {
            var user = Mapper.Map<RegisterViewModel, User>(model as RegisterViewModel);
            var result = await _userManagerFacade.CreateUserAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManagerFacade.AddToRoleAsync(user.Id, nameof(Roles.User));
                await _signInManagerFacade.SignInAsync(user);
                await SendConfirmationEmail(user);
            }
            return result;
        }

        public User GetByName(string name)
        {
            return _userManagerFacade.FindByNameAsync(name).Result as User;
        }

        public async Task<User> GetByNameAsync(string name)
        {
            return await _userManagerFacade.FindByNameAsync(name) as User;
        }

        private async Task SendConfirmationEmail(User user)
        {
            string code = await _userManagerFacade.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = _urlHelperFacade.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: _urlHelperFacade.GetUrlScheme());
            await _userManagerFacade.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
        }

        private async Task SendResetPasswordEmail(User user)
        {
            string code = await _userManagerFacade.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = _urlHelperFacade.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: _urlHelperFacade.GetUrlScheme());
            await _userManagerFacade.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
        }

        public async Task<IdentityResult> UpdatePassword(IResetPasswordViewModel model)
        {
            var user = await _userManagerFacade.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new IdentityResult("We did not find user, Perhaps it was deleted or blocked, Please inform customer support!");
            }
            return await _userManagerFacade.ResetPasswordAsync(user.Id, model.Code, model.Password);
        }

        public async Task<IdentityResult> ForgotPassword(IForgotPasswordViewModel model)
        {
            var user = await GetByNameAsync(model.Email);
            if (user == null)
                return new IdentityResult($"The {model.Email} not registired to our service!");

            await SendResetPasswordEmail(user);
            return IdentityResult.Success;
        }

        public Task<IdentityResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
                return Task.FromResult(new IdentityResult("code or userId is empty"));

            return _userManagerFacade.ConfirmEmailAsync(userId, code);
        }

        public Task<SignInStatus> SignIn(ILoginViewModel model)
        {
            return _signInManagerFacade.PasswordSignInAsync(model.Email, model.Password, model.RememberMe);
        }
    }
}
