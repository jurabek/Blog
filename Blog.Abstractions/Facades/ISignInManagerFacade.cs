using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Blog.Abstractions.Facades
{
    public interface ISignInManagerFacade<in TUser> where TUser : IUser
    {
        Task<SignInStatus> PasswordSignInAsync(string email, string password, bool rememberMe);
        Task SignInAsync(TUser user);
        void SignOut();
    }
}