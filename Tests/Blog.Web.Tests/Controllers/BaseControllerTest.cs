using NUnit.Framework;
using System;
using System.Web.Mvc;

namespace Blog.Web.Tests.Controllers
{
    [TestFixture]
    public abstract class BaseControllerTest<TController> : IDisposable 
        where TController : Controller
    {
        protected TController _controller;

        [OneTimeTearDown]
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [OneTimeSetUp]
        public abstract void Init();

        public virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                _controller.Dispose();
                Dispose();
            }
        }
        protected void ClearModelState()
        {
            _controller.ModelState.Clear();            
        }
    }
}
