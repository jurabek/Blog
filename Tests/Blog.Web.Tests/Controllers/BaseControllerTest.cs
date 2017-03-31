using NUnit.Framework;
using System;
using System.Web.Mvc;
using Moq;

namespace Blog.Web.Tests.Controllers
{
    [TestFixture]
    public abstract class BaseControllerTest<TController, TRepository> : IDisposable 
        where TController : Controller
        where TRepository : class
    {
        protected TController _controller;
        protected Mock<TRepository> _repository;

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
                Dispose();
            }
        }
        protected void ClearModelState()
        {
            _controller.ModelState.Clear();            
        }
    }
}
