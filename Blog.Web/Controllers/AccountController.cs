using Blog.Abstractions.Fasades;
using Blog.Data.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Blog.Data.Entities;

namespace Blog.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IUserManagerFacade _userManagerFacade;
        private ISignInManagerFacade _signInManagerFacade;

        public AccountController(IUserManagerFacade userManagerFacade, ISignInManagerFacade signInManagerFacade)
        {
            _userManagerFacade = userManagerFacade;
            _signInManagerFacade = signInManagerFacade;
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
                var result = await _signInManagerFacade.PasswordSignInAsync(model.Email, model.Password, model.RememberMe);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(redirectUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }
            return View();
        }


        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        private const string SuccessTitle = "success_title";
        private const string SuccessMessage = "success_body";

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = model.Name,
                    LastName = model.LastName,
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await _userManagerFacade.CreateUserAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManagerFacade.SignInAsync(user);

                    string code = await _userManagerFacade.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await _userManagerFacade.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    TempData[SuccessTitle] = "Email confirmation";
                    TempData[SuccessMessage] = "Email confirmation has been sent, please check your email!";

                    return RedirectToAction("Success", "Account");
                }
                AddErrors(result);
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Success()
        {
            ViewBag.Title = TempData[SuccessTitle];
            ViewBag.Message = TempData[SuccessMessage];

            TempData.Remove(SuccessTitle);
            TempData.Remove(SuccessMessage);
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await _userManagerFacade.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            var result = _userManagerFacade.ChangePassword(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                TempData[SuccessTitle] = "Password changed";
                TempData[SuccessMessage] = "Your password successfully changed!";
                return RedirectToAction("Success", "Account");
            }
            return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}