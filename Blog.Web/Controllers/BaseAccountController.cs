using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace Blog.Web.Controllers
{
    public abstract class BaseAccountController : Controller
    {
        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}