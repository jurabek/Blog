using Blog.Core.Managers;
using Blog.Model;
using Blog.Web.App_Start;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Blog.Web
{
    public static class Extensions
    {
        private static ApplicationUserManager userManager = StructuremapBootstrapper.Container.GetInstance<ApplicationUserManager>();

        public static string GetUserTitle(this IIdentity identity)
        {
            var user = userManager.FindById(identity.GetUserId());
            if (user != null)
            {
                if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.LastName))
                    return user.UserName;
                return user.FullName;
            }
            return string.Empty;
        }
        
    }
}