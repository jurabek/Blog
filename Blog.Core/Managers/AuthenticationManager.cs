using System.Threading.Tasks;
using Blog.Abstractions.Facades;
using Blog.Abstractions.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Blog.Abstractions.Managers;
using Blog.Model.Entities;

namespace Blog.Core.Managers
{
    public class AuthenticationManager : IAuthenticationManager<User>
    {
        private readonly ISignInManagerFacade<User> _signInManagerFacade;
        
        public AuthenticationManager(ISignInManagerFacade<User> signInManagerFacade)
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
        public Task SignInAsync(User user)
        {
            return _signInManagerFacade.SignInAsync(user);
        }

    }
}