using AutoMapper;
using Blog.Abstractions.Fasades;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Blog.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private IUserManagerFacade _userManagerFacade;
        private ISignInManagerFacade _signInManagerFacade;

        public ManageController(IUserManagerFacade userManagerFacade, ISignInManagerFacade signInManagerFacade)
        {
            _userManagerFacade = userManagerFacade;
            _signInManagerFacade = signInManagerFacade;
        }

        // GET: Manage
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
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            var result = _userManagerFacade.ChangePassword(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            return View();
        }

        public async Task<ActionResult> ChangeProfile()
        {
            var user = await _userManagerFacade.FindByIdAsync(User.Identity.GetUserId()) as User;
            var model = Mapper.Map<User, ChangeProfileViewModel>(user);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeProfile(ChangeProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                await UpdateUserProfile(model);
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangeProfileSuccess });
            }
            return View();
        }

        private async Task UpdateUserProfile(ChangeProfileViewModel model)
        {
            var user = await _userManagerFacade.FindByIdAsync(User.Identity.GetUserId()) as User;
            user.Name = model.Name;
            user.LastName = model.LastName;
            await _userManagerFacade.UpdateAsync(user);
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            ChangeProfileSuccess,
            Error
        }
    }
}