using NUnit.Framework;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Web.Tests.Controllers
{
    [TestFixture]
    public abstract class BaseControllerTest : IDisposable
    {
        public IContainer Container { get; protected set; }

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
    }
}
