using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Model.ViewModels;
using Blog.Model.Entities;
using Blog.Abstractions.Repositories;
using Blog.Abstractions.Managers;
using Blog.Core.Managers;
using Blog.Abstractions.Facades;

namespace Blog.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IAuthenticationManager<User> _authenticationManager;
        private readonly IUserManager _userManager;

        public AccountController(
            IUserManager userManager,
            IAuthenticationManager<User> authenticationManager, 
            IUrlHelperFacade urlHelperFacade) : base(urlHelperFacade)
        {
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }

        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl = null)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string ReturnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _authenticationManager.SignIn(model);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(ReturnUrl);

                    case SignInStatus.Failure:
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
                var result = await _userManager.SendResetPasswordEmail(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Success", "Account", new SuccessViewModel
                    {
                        Title = "Forgot Password Confirmation",
                        Message = "Please check your email to reset your password."
                    });
                }
                AddErrors(result);
            }
            return View(model);
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
                var result = await _userManager.SignUpAndSignIn(model);
                if (result.Succeeded)
                {
                    var successModel = new SuccessViewModel
                    {
                        Title= "Email confirmation",
                        Message = "Email confirmation has been sent, please check your email!"
                    };
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
                var result = await _userManager.ResetPassword(model);
                if (result.Succeeded)
                {
                    return View("ResetPasswordConfirmation");
                }
                AddErrors(result);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _authenticationManager.SignOut();
            return RedirectToAction("Index", "Article");
        }

        [AllowAnonymous]
        public ActionResult Success(SuccessViewModel model)
        {
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            var result = await _userManager.ConfirmEmail(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
    }
}