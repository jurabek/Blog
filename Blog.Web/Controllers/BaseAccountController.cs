using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Web.Controllers
{
    public abstract class BaseAccountController : Controller
    {
        protected readonly IAccountRepository<User, string> _accountRepository;

        protected BaseAccountController(IAccountRepository<User, string> accountRepository)
        {
            _accountRepository = accountRepository;
        }

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