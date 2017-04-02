﻿using Blog.Abstractions.Facades;
using Blog.Abstractions.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Managers
{
    public class EmailManager : IEmailManager
    {
        private IUrlHelperFacade _urlHelperFacade;
        private IUserManagerFacade _userManagerFacade;

        public EmailManager(IUserManagerFacade userManagerFacade, IUrlHelperFacade urlHelperFacade)
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
