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
using Blog.Core.Helpers;

namespace Blog.Web
{
    public static class Extensions
    {
        private static ApplicationUserManager userManager = StructuremapBootstrapper.Container.GetInstance<ApplicationUserManager>();

        public static string GetUserTitle(this IIdentity identity)
        {
            var user = userManager.FindById(identity.GetUserId());
            if (user != null)
                return user.FullName;

            return string.Empty;
        }

        public static bool HasPermission(this IIdentity identity, Permission permission)
        {
            var user = userManager.FindById(identity.GetUserId());
            return false;
        }

        public static IHtmlString DisplayForPermission<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> ex)
            where TValue : struct, IConvertible
        {
            var enums = (Permission)ModelMetadata.FromLambdaExpression(ex, html.ViewData).Model;

            var flags = enums.GetFlags();            
            
            string result = 
                string.Concat(flags.Select(x => GetCheckBox(x.GetEnumDescription())));

            return html.Raw(result);
        }

        private static string GetCheckBox(string text)
        {
            return string.Format(@"
                                <div class='checkbox'>
                                  <label><input checked='checked' disabled='disabled' class='check-box' type='checkbox'>{0}</label>
                                 </div>", text);
        }       
    }
}