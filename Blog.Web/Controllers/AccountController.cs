using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Model.ViewModels;
using Blog.Model.Entities;
using System.Web;
using AutoMapper;
using Blog.Core.Managers;
using IdentityPermissionExtension;
using Blog.Abstractions.Facades;
using Blog.Abstractions.Repositories;

namespace Blog.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseAccountController
    {

        public AccountController(IAccountRepository<User, string> accountRepository) 
            : base(accountRepository)
        {
        }

        [AllowAnonymous]
        public ActionResult Login(string redirectUrl)
        {
            ViewBag.RedirecrUrl = redirectUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.SignIn(model);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(redirectUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                    }
                }
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.ForgotPassword(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
                AddErrors(result);
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.CreateUserAsync(model);
                if (result.Succeeded)
                {
                    var successModel = new SuccessViewModel
                                       (
                                         "Email confirmation",
                                         "Email confirmation has been sent, please check your email!"
                                       );

                    return RedirectToAction("Success", "Account", successModel);
                }
                AddErrors(result);
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string userId = null, string code = null)
        {
            return code == null ? View("Error")
                                : View(new ResetPasswordViewModel { UserId = userId });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.ResetPassword(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                AddErrors(result);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Success(SuccessViewModel model)
        {
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            var result = await _accountRepository.ConfirmEmail(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
    }
}