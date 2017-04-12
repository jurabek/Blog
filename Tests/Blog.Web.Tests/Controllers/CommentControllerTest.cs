using Blog.Abstractions.Providers;
using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using Blog.Web.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Blog.Web.Tests.Controllers
{
    [TestFixture]
    public class CommentControllerTest : BaseControllerTest<CommentController>
    {
        private Mock<IRepository<Comment, string, bool>> _repository;
        private Mock<IUserPermissionProvider> _permissionProvider;

        public override void Init()
        {
            _repository = new Mock<IRepository<Comment, string, bool>>();
            _permissionProvider = new Mock<IUserPermissionProvider>();
            _controller = new CommentController(_repository.Object, _permissionProvider.Object);

            _permissionProvider.Setup(x => x.CheckUserPermission(It.IsAny<string>(),
                It.IsAny<string>()))
                .Returns(Task.FromResult(true));
        }

        [Test]
        public void Create_should_return_json_object()
        {
            var result = _controller.Create("articleId", "userId", "message") as JsonResult;

            Assert.That(result.Data.GetType(), Is.EqualTo(typeof(Comment)));
        }

        [Test]
        public async Task Can_user_write_comment_should_return_true()
        {
            var result = await _controller.CanUserWriteComment() as JsonResult;
            Assert.That(result.Data, Is.EqualTo(true));
        }

        [Test]
        public async Task Can_user_delete_comment_should_return_result()
        {
            var result = await _controller.CanUserDeleteComment("test") as JsonResult;
            Assert.That(result.Data, Is.EqualTo(false));
        }

        [Test]
        public async Task Can_user_edit_comment_should_return_result()
        {
            var result = await _controller.CanUserEditComment("test") as JsonResult;
            Assert.That(result.Data, Is.EqualTo(false));
        }
    }
}
