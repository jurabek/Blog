using Blog.Abstractions.Facades;
using Blog.Core.Managers;
using Blog.Model.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;

namespace Blog.Core.Fasades
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SignInManagerFacade : ISignInManagerFacade<User>
    {
        private readonly IdentitySignInManager _signInManager;

        public SignInManagerFacade(IdentitySignInManager signInManager)
        {
            _signInManager = signInManager;
        }

        public Task<SignInStatus> PasswordSignInAsync(string email, string password, bool rememberMe)
        {
            return _signInManager.PasswordSignInAsync(email, password, rememberMe, false);
        }

        public Task SignInAsync(User user)
        {
            return _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        }

        public void SignOut()
        {
            _signInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

    }
}