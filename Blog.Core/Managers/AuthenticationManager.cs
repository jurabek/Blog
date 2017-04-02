using System.Threading.Tasks;
using Blog.Abstractions.Facades;
using Blog.Abstractions.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Blog.Abstractions.Managers;

namespace Blog.Core.Managers
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly ISignInManagerFacade _signInManagerFacade;
        
        public AuthenticationManager(ISignInManagerFacade signInManagerFacade)
        {
            _signInManagerFacade = signInManagerFacade;
        }

        /// <summary>
        /// Signs via model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<SignInStatus> SignIn(ILoginViewModel model)
        {
            return _signInManagerFacade.PasswordSignInAsync(model.Email, model.Password, model.RememberMe);
        }

        /// <summary>
        /// Sign outs from session
        /// </summary>
        public void SignOut()
        {
            _signInManagerFacade.SignOut();
        }

        /// <summary>
        /// Signs via user intance
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task SignInAsync(IUser user)
        {
            return _signInManagerFacade.SignInAsync(user);
        }

    }
}