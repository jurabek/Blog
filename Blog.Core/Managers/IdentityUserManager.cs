using Microsoft.AspNet.Identity;
using System;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Blog.Core.Services;
using Blog.Model.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Core.Managers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ApplicationUserManager : UserManager<User, string>
    {
        public ApplicationUserManager(IdentityUserStore store, IDataProtectionProvider dataProtectionProvider) : base(store)
        {
            this.UserValidator = new UserValidator<User>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            this.PasswordValidator = new PasswordValidator { RequiredLength = 6 };            
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;

            this.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            this.EmailService = new EmailService();
            
#if !DEBUG_TEST
            if (dataProtectionProvider != null)
            {
                IDataProtector dataProtector = dataProtectionProvider.Create("ASP.NET Identity");

                this.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtector);
            }
#endif
        }
    }
}