using AutoMapper;
using Blog.Abstractions.Facades;
using Blog.Abstractions.Managers;
using Blog.Abstractions.Repositories;
using Blog.Core.Managers;
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
        private IUserManager _userManager;
        private IMappingManager _mappingManager;
        private IUserRepository<User, string> _repository;

        public ManageAccountController(IUserRepository<User, string> userRepository,
            IUserManager userManager,
            IMappingManager mappingManager)
        {
            _repository = userRepository;
            _userManager = userManager;
            _mappingManager = mappingManager;
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
                var result = await _userManager.UpdatePassword(User?.Identity?.GetUserId(), model);
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
            var user = await _repository.GetAsync(User?.Identity?.GetUserId());
            var model = _mappingManager.Map<User, UpdateProfileViewModel>(user);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeProfile(UpdateProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.UpdateProfile(User?.Identity?.GetUserId(), model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", new { Message = ManageMessageId.ChangeProfileSuccess });
                }
                AddErrors(result);
            }
            return View(model);
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            ChangeProfileSuccess,
            Error
        }
    }
}