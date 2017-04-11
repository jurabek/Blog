using Blog.Abstractions.Providers;
using Blog.Abstractions.Repositories;
using Blog.Model;
using Blog.Model.Entities;
using IdentityPermissionExtension;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;

namespace Blog.Web.Controllers
{
    public class CommentController : Controller
    {
        private IRepository<Comment, string, bool> _repository;
        private IUserPermissionProvider _userPermissionProvider;

        public CommentController(
            IRepository<Comment, string, bool> repository,
            IUserPermissionProvider userPermissionProvider)
        {
            _repository = repository;
            _userPermissionProvider = userPermissionProvider;
        }

        [AuthorizePermission(Roles = "Administrator", Name = nameof(Permissions.CanWriteComment))]
        [HttpPost]
        public ActionResult Create(string articleId, string userId, string message)
        {
            var comment = new Comment
            {
                ArticleId = articleId,
                UserId = userId,
                Message = message,
                PublishDate = DateTime.Now,
            };
            _repository.Add(comment);
            return Json(comment);
        }

        [HttpPost]
        public async Task<ActionResult> CanUserWriteComment()
        {
            var result = await _userPermissionProvider.CheckUserPermission(
                User.Identity.GetUserId(), 
                nameof(Permissions.CanWriteComment));

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> CanUserDeleteComment(string authorId)
        {
            var hasPermission = await _userPermissionProvider.CheckUserPermission(
                User.Identity.GetUserId(),
                nameof(Permissions.CanDeleteComment));

            var result = hasPermission && User.Identity.GetUserId() == authorId;

            return Json(result);
        }

        public ActionResult CanUserEditComment(string userId)
        {
            return Json(false);
        }
    }
}