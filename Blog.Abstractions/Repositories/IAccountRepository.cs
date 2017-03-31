using Blog.Abstractions.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Abstractions.Repositories
{
    public interface IAccountRepository<T, TKey>
        where T : class, IUser 
    {
        Task<T> GetAsync(TKey id);
        T Get(TKey id);
        Task<T> GetByNameAsync(string name);
        T GetByName(string name);
        Task<IdentityResult> CreateUserAsync(IRegisterUserViewModel model);
        Task<IdentityResult> ResetPassword(IResetPasswordViewModel model);
        Task<IdentityResult> ForgotPassword(IForgotPasswordViewModel model);
        Task<IdentityResult> UpdatePassword(TKey userId, IUpdatePasswordViewModel model);
        Task<IdentityResult> UpdateProfile(TKey userId, IUpdateProfileViewModel model);
        Task<IdentityResult> ConfirmEmail(TKey userId, string code);
        Task<SignInStatus> SignIn(ILoginViewModel model);
        void SignOut();
        Task SendConfirmationEmail(TKey userId);
        Task SendResetPasswordEmail(TKey userId);
    }
}
