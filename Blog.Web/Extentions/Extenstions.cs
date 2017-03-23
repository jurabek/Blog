using Blog.Core.Managers;
using Blog.Web.App_Start;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Blog.Web
{
    public static class Extensions
    {
        public static string GetUserTitle(this IIdentity identity)
        {
            var userManager = StructuremapBootstrapper.Container.GetInstance<ApplicationUserManager>();

            var user = userManager.FindById(identity.GetUserId());
            if (user != null)
                return user.FullName;

            return string.Empty;
        }
    }
}