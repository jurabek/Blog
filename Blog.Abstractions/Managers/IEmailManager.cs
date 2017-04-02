using System.Threading.Tasks;

namespace Blog.Abstractions.Managers
{
    public interface IEmailManager
    {
        Task SendConfirmationEmail(string userId);
        Task SendResetPasswordEmail(string userId);
    }
}