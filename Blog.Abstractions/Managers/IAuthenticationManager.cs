using System.Threading.Tasks;
using Blog.Abstractions.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Blog.Abstractions.Managers
{
    public interface IAuthenticationManager<in TUser> where TUser : IUser
    {

        Task<SignInStatus> SignIn(ILoginViewModel model);

        Task SignInAsync(TUser user);

        void SignOut();
    }
}