using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Linq;

namespace Blog.Abstractions.Facades
{
    public interface IUserManagerFacade<TUser> where TUser : IUser
    {
        IQueryable<TUser> Users { get; }

        Task<TUser> FindByIdAsync(string userId);

        Task<TUser> FindByNameAsync(string name);

        Task<IdentityResult> CreateAsync(TUser user);

        Task<IdentityResult> CreateAsync(TUser user, string password);

        Task<IdentityResult> UpdateAsync(TUser user);

        Task<IdentityResult> DeleteAsync(TUser user);

        Task<IdentityResult> ChangePassword(string userId, string oldPassword, string newPassword);
        
        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);

        Task<string> GenerateEmailConfirmationTokenAsync(string userId);

        Task SendEmailAsync(string userId, string subject, string body);
        
        Task<string> GeneratePasswordResetTokenAsync(string userId);

        Task<IdentityResult> ResetPasswordAsync(string id, string code, string password);

        Task<IdentityResult> AddToRoleAsync(string userId, string role);

        Task<IdentityResult> AddToRolesAsync(string userId, params string[] roles);

        Task<IdentityResult> RemoveFromRoleAsync(string userId, string role);

        Task<IdentityResult> RemoveFromRolesAsync(string userId, params string[] roles);

        Task<bool> IsInRoleAsync(string userId, string roleId);

    }
}