using Blog.Abstractions.Facades;
using Blog.Abstractions.Managers;
using Blog.Model.Entities;
using System.Threading.Tasks;

namespace Blog.Core.Managers
{
    public class EmailManager : IEmailManager
    {
        private readonly IUrlHelperFacade _urlHelperFacade;
        private readonly IUserManagerFacade<User> _userManagerFacade;

        public EmailManager(IUserManagerFacade<User> userManagerFacade, IUrlHelperFacade urlHelperFacade)
        {
            _userManagerFacade = userManagerFacade;
            _urlHelperFacade = urlHelperFacade;
        }

        public async Task SendConfirmationEmail(string userId)
        {
            string code = await _userManagerFacade.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = _urlHelperFacade.Action("ConfirmEmail", "Account", new { userId = userId, code = code }, protocol: _urlHelperFacade.GetUrlScheme());
            await _userManagerFacade.SendEmailAsync(userId, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
        }

        public async Task SendResetPasswordEmail(string userId)
        {
            string code = await _userManagerFacade.GeneratePasswordResetTokenAsync(userId);
            var callbackUrl = _urlHelperFacade.Action("ResetPassword", "Account", new { userId = userId, code = code }, protocol: _urlHelperFacade.GetUrlScheme());
            await _userManagerFacade.SendEmailAsync(userId, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
        }
    }
}
