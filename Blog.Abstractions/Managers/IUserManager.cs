using System.Threading.Tasks;
using Blog.Abstractions.ViewModels;
using Microsoft.AspNet.Identity;

namespace Blog.Core.Managers
{
    public interface IUserManager
    {
        Task<IdentityResult> SignUpAndSignIn(IRegisterUserViewModel model);
        Task<IdentityResult> AddToRoleAsync(string userId, string roleName);
        Task<IdentityResult> ConfirmEmail(string userId, string code);
        Task<bool> IsInRoleAsync(string userId, string roleName);
        Task<IdentityResult> ResetPassword(IResetPasswordViewModel model);
        Task<IdentityResult> SendResetPasswordEmail(IForgotPasswordViewModel model);
        Task<IdentityResult> UpdatePassword(string userId, IUpdatePasswordViewModel model);
        Task<IdentityResult> UpdateProfile(string userId, IUpdateProfileViewModel model);
    }
}