using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Core.Mappings;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using NUnit.Framework;
using Blog.Abstractions.Managers;
using Blog.Core.Managers;

namespace Blog.Web.Tests.Mappings
{
    [TestFixture]
    public class MappingManagerTest : IDisposable
    {
        private IMappingManager mappingManager;
        
        [OneTimeSetUp]
        public void Init()
        {
            AutoMapperConfiguration.Configure();
            mappingManager = new MappingManager();
        }

        [OneTimeTearDown]
        public void Dispose()
        {
        }

        [Test]
        public void ModelToViewModelWithMappingManagerTest()
        {
            var user = new User
            {
                Name = "Map Name",
                LastName = "Map LastName"
            };

            var vm = mappingManager.Map<User, UpdateProfileViewModel>(user);

            Assert.AreEqual("Map Name", vm.Name);
            Assert.AreEqual("Map LastName", vm.LastName);
        }
    }
}
