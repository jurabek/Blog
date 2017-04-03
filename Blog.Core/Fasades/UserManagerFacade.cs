using Blog.Core.Managers;
using Blog.Model.Entities;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System;
using Blog.Abstractions.Facades;
using System.Linq;

namespace Blog.Core.Fasades
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class UserManagerFacade : IUserManagerFacade<User>
    {
        private readonly ApplicationUserManager _userManager;

        public UserManagerFacade(ApplicationUserManager userManager)
        {
            _userManager = userManager;

        }
        public IQueryable<User> Users { get { return _userManager.Users; } }
        
        public async Task<User> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<User> FindByNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }
        
        public Task<IdentityResult> CreateAsync(User user)
        {
            return _userManager.CreateAsync(user);
        }

        public Task<IdentityResult> CreateAsync(User user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public Task<IdentityResult> UpdateAsync(User user)
        {
            return _userManager.UpdateAsync(user);
        }

        public Task<IdentityResult> DeleteAsync(User user)
        {
            return _userManager.DeleteAsync(user);
        }

        public Task<IdentityResult> ChangePassword(string userId, string oldPassword, string newPassword)
        {
            return _userManager.ChangePasswordAsync(userId, oldPassword, newPassword);
        }

        public Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            return _userManager.GenerateEmailConfirmationTokenAsync(userId);
        }

        public Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            return _userManager.ConfirmEmailAsync(userId, code);
        }

        public Task SendEmailAsync(string userId, string subject, string body)
        {
            return _userManager.SendEmailAsync(userId, subject, body);
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

        public Task<IdentityResult> AddToRolesAsync(string userId, params string[] roles)
        {
            return _userManager.AddToRolesAsync(userId, roles);
        }

        public Task<IdentityResult> RemoveFromRoleAsync(string userId, string role)
        {
            return _userManager.RemoveFromRoleAsync(userId, role);
        }

        public Task<IdentityResult> RemoveFromRolesAsync(string userId, params string[] roles)
        {
            return _userManager.RemoveFromRolesAsync(userId, roles);
        }

        public Task<bool> IsInRoleAsync(string userId, string roleName)
        {
            return _userManager.IsInRoleAsync(userId, roleName);
        }
    }
}