using Blog.Abstractions.Facades;
using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace Blog.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        private IUrlHelperFacade _urlHelperFacade;

        protected BaseController(IUrlHelperFacade urlHelperFacade)
        {
            _urlHelperFacade = urlHelperFacade;
        }
        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && _urlHelperFacade.IsLocalUrl(returnUrl))
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