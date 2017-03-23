using Blog.Abstractions.Fasades;
using Blog.Core.Managers;
using Blog.Data.Entities;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Blog.Core.Fasades
{
    public class UserManagerFacade : IUserManagerFacade
    {
        private ApplicationUserManager _userManager;

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
    }
}