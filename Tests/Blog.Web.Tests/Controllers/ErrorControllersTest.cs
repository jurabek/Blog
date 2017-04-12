using Blog.Web.Controllers;
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
    public class ErrorControllersTest : BaseControllerTest<ErrorController>
    {
        public override void Init()
        {
            _controller = new ErrorController();
        }

        [Test]
        public void Access_denied_should_return_view()
        {
            var result = _controller.AccessDenied();

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
