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
        Task<T> GetByNameAsync(string name);
        T GetByName(string name);
        Task<IdentityResult> CreateUserAsync(IUserViewModel model);
        Task<IdentityResult> UpdatePassword(IResetPasswordViewModel model);
        Task<IdentityResult> ForgotPassword(IForgotPasswordViewModel model);
        Task<IdentityResult> ConfirmEmail(string userId, string code);
        Task<SignInStatus> SignIn(ILoginViewModel model);
    }
}
