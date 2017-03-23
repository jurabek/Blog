using Blog.Abstractions.Fasades;
using Blog.Core.Managers;
using Blog.Data.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Blog.Core.Fasades
{
    public class SignInManagerFacade : ISignInManagerFacade
    {
        private ApplicationSignInManager _signInManager;

        public SignInManagerFacade(ApplicationSignInManager signInManager)
        {
            _signInManager = signInManager;
        }

        public Task<SignInStatus> PasswordSignInAsync(string email, string password, bool rememberMe)
        {
            return _signInManager.PasswordSignInAsync(email, password, rememberMe, false);
        }

        public Task SignInAsync(IUser user)
        {
            return _signInManager.SignInAsync(user as User, isPersistent: false, rememberBrowser: false);
        }
    }
}