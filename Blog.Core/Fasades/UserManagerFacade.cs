using Blog.Core.Managers;
using Blog.Model.Entities;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System;
using Blog.Abstractions.Facades;

namespace Blog.Core.Fasades
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class UserManagerFacade : IUserManagerFacade
    {
        private readonly ApplicationUserManager _userManager;

        public UserManagerFacade(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public Task SendEmailAsync(string userId, string subject, string body)
        {
           return _userManager.SendEmailAsync(userId, subject, body);
        }

        public Task<IdentityResult> CreateUserAsync(IUser user, string password)
        {
            return _userManager.CreateAsync(user as User, password);
        }

        public Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            return _userManager.GenerateEmailConfirmationTokenAsync(userId);
        }

        public Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            return _userManager.ConfirmEmailAsync(userId, code);
        }

        public IdentityResult ChangePassword(string userId, string oldPassword, string newPassword)
        {
            return _userManager.ChangePassword(userId, oldPassword, newPassword);
        }

        public Task<IdentityResult> UpdateAsync(IUser user)
        {
            return _userManager.UpdateAsync(user as User);
        }

        public async Task<IUser> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IUser> FindByNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public Task<string> GeneratePasswordResetTokenAsync(string userId)
        {
            return _userManager.GeneratePasswordResetTokenAsync(userId);
        }

        public Task<IdentityResult> ResetPasswordAsync(string id, string code, string password)
        {
            return _userManager.ResetPasswordAsync(id, code, password);
        }


        public Task<IdentityResult> AddToRoleAsync(string userId, string role)
        {
            return _userManager.AddToRoleAsync(userId, role);
        }

    }
}