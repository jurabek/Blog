using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Linq;

namespace Blog.Abstractions.Facades
{
    public interface IUserManagerFacade
    {
        IQueryable<IUser> Users { get; }

        Task<IUser> FindByIdAsync(string userId);

        Task<IUser> FindByNameAsync(string name);

        Task<IdentityResult> CreateAsync(IUser user);

        Task<IdentityResult> CreateAsync(IUser user, string password);

        Task<IdentityResult> UpdateAsync(IUser user);

        Task<IdentityResult> DeleteAsync(IUser user);

        Task<IdentityResult> ChangePassword(string userId, string oldPassword, string newPassword);
        
        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);

        Task<string> GenerateEmailConfirmationTokenAsync(string userId);

        Task SendEmailAsync(string userId, string subject, string body);
        
        Task<string> GeneratePasswordResetTokenAsync(string userId);

        Task<IdentityResult> ResetPasswordAsync(string id, string code, string password);

        Task<IdentityResult> AddToRoleAsync(string userId, string role);

        Task<bool> IsInRoleAsync(string userId, string roleId);

    }
}