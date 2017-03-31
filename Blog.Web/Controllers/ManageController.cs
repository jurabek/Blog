using AutoMapper;
using Blog.Abstractions.Facades;
using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Blog.Web.Controllers
{
    [Authorize]
    public class ManageAccountController : BaseAccountController
    {
        public ManageAccountController(IAccountRepository<User, string> accountRepository) 
            : base(accountRepository)
        {
        }

        public ActionResult Index(ManageMessageId? message = null)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.ChangeProfileSuccess ? "Your profile has been changed"
                : "";

            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(UpdatePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.UpdatePassword(User.Identity.GetUserId(), model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                AddErrors(result);
            }
            return View(model);
        }

        public async Task<ActionResult> ChangeProfile()
        {
            var user = await _accountRepository.GetAsync(User.Identity.GetUserId());
            var model = Mapper.Map<User, UpdateProfileViewModel>(user);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeProfile(UpdateProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.UpdateProfile(User.Identity.GetUserId(), model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", new { Message = ManageMessageId.ChangeProfileSuccess });
                }
                AddErrors(result);
            }
            return View();
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            ChangeProfileSuccess,
            Error
        }
    }
}