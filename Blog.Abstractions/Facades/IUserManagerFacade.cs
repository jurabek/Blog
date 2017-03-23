using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Blog.Abstractions.Fasades
{
    public interface IUserManagerFacade
    {
        IdentityResult ChangePassword(string userId, string oldPassword, string newPassword);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);
        Task<IdentityResult> CreateUserAsync(IUser user, string password);
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task SendEmailAsync(string userId, string subject, string body);
    }
}