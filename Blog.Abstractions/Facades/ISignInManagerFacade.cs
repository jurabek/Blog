using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Blog.Abstractions.Fasades
{
    public interface ISignInManagerFacade
    {
        Task<SignInStatus> PasswordSignInAsync(string email, string password, bool rememberMe);
        Task SignInAsync(IUser user);
    }
}