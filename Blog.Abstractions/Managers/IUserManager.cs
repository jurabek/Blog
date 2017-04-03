using System.Threading.Tasks;
using Blog.Abstractions.ViewModels;
using Microsoft.AspNet.Identity;

namespace Blog.Abstractions.Managers
{
    public interface IUserManager
    {
        Task<IdentityResult> SignUpAndSignIn(IRegisterUserViewModel model);
        Task<IdentityResult> AddToRoleAsync(string userId, string roleName);
        Task<IdentityResult> AddToRolesAsync(string userId, params string[] roles);
        Task<IdentityResult> RemoveFromRoleAsync(string userId, string role);
        Task<IdentityResult> RemoveFromRolesAsync(string userId, params string[] roles);
        Task<IdentityResult> ConfirmEmail(string userId, string code);
        Task<bool> IsInRoleAsync(string userId, string roleName);
        Task<IdentityResult> ResetPassword(IResetPasswordViewModel model);
        Task<IdentityResult> SendResetPasswordEmail(IForgotPasswordViewModel model);
        Task<IdentityResult> UpdatePassword(string userId, IUpdatePasswordViewModel model);
        Task<IdentityResult> UpdateProfile(string userId, IUpdateProfileViewModel model);
    }
}