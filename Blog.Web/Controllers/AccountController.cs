using Blog.Abstractions.Fasades;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Model.ViewModels;
using Blog.Model.Entities;
using System.Web;
using Microsoft.Owin.Security;
using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using Blog.Core.Managers;
using IdentityPermissionExtension;

namespace Blog.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private const string SuccessTitle = "success_title";
        private const string SuccessMessage = "success_body";

        private IUserManagerFacade _userManagerFacade;
        private ISignInManagerFacade _signInManagerFacade;
        private IdentityRoleManager _roleManager;

        public AccountController(IUserManagerFacade userManagerFacade, ISignInManagerFacade signInManagerFacade, IdentityRoleManager roleManager)
        {
            _userManagerFacade = userManagerFacade;
            _signInManagerFacade = signInManagerFacade;
            _roleManager = roleManager;
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
                var user = await _userManagerFacade.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", $"The {model.Email} not registired to our service!");
                    return View(model);
                }
                string code = await _userManagerFacade.GeneratePasswordResetTokenAsync(user.Id);

                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                await _userManagerFacade.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
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
                var user = Mapper.Map<RegisterViewModel, User>(model);

                var result = await _userManagerFacade.CreateUserAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManagerFacade.SignInAsync(user);

                    string code = await _userManagerFacade.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await _userManagerFacade.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    var userRole = _roleManager.FindByName(nameof(Roles.User));

                    await _userManagerFacade.AddToRoleAsync(user.Id, userRole.Name);

                    AddSuccessTempData("Email confirmation", "Email confirmation has been sent, please check your email!");

                    return RedirectToAction("Success", "Account");
                }
                AddErrors(result);
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string userId = null, string code = null)
        {
            return code == null ? View("Error") : View(new ResetPasswordViewModel { UserId = userId });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManagerFacade.FindByIdAsync(model.UserId);
            if (user == null)
            {
                ModelState.AddModelError("", "We did not find user, Perhaps it was deleted or blocked, Please inform customer suppor about that!");
                return View();
            }

            var result = await _userManagerFacade.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Success()
        {
            ViewBag.Title = TempData[SuccessTitle];
            ViewBag.Message = TempData[SuccessMessage];
            ClearSuccessTempData();
            return View();
        }

        private void ClearSuccessTempData()
        {
            TempData.Remove(SuccessTitle);
            TempData.Remove(SuccessMessage);
        }

        private void AddSuccessTempData(string title, string message)
        {
            TempData[SuccessTitle] = title;
            TempData[SuccessMessage] = message;
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