using System.Threading.Tasks;
using Blog.Abstractions.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Blog.Abstractions.Managers
{
    public interface IAuthenticationManager
    {
        /// <summary>
        /// Signs via model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SignInStatus> SignIn(ILoginViewModel model);

        /// <summary>
        /// Signs via user intance
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task SignInAsync(IUser user);

        /// <summary>
        /// Sign outs from session
        /// </summary>
        void SignOut();
    }
}