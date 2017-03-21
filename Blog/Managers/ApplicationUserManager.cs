using Microsoft.AspNet.Identity;
using System;
using Microsoft.AspNet.Identity.Owin;
using Blog.Models;
using Microsoft.Owin.Security.DataProtection;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace Blog.Managers
{
    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> store) : base(store)
        {
            this.UserValidator = new UserValidator<User>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two-factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            this.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User>
            {
                MessageFormat = "Your security code is {0}"
            });

            this.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            this.EmailService = new EmailService();

            //TODO: set sms service here
            //this.SmsService = new SmsService();


            var dataProtectionProvider = Startup.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                IDataProtector dataProtector = dataProtectionProvider.Create("ASP.NET Identity");

                this.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtector);
            }
        }

        public async Task<IdentityResult> ChangePasswordAsync(User userId, string newPassword)
        {
            var store = this.Store as IUserPasswordStore<User>;
            if (store == null)
            {
                var errors = new string[]
                {
                "Current UserStore doesn't implement IUserPasswordStore"
                };

                return await Task.FromResult<IdentityResult>(new IdentityResult(errors));
            }

            var newPasswordHash = this.PasswordHasher.HashPassword(newPassword);

            await store.SetPasswordHashAsync(userId, newPasswordHash);
            return await Task.FromResult<IdentityResult>(IdentityResult.Success);
        }
    }
}