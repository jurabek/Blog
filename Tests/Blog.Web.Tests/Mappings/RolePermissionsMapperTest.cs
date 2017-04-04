using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Abstractions.Fasades;
using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using Moq;
using NUnit.Framework;

namespace Blog.Web.Tests.Mappings
{
    [TestFixture]
    public class RolePermissionsMapperTest
    {
        private Mock<IUserRepository<User, string>> _userRepository;
        private Mock<IRoleRepository<IdentityRole, string>> _roleRepository;
        private Mock<IPermissionManagerFacade<IdentityPermission>> _permissionManagerFacade;
        [OneTimeSetUp]
        public void Init()
        {
            _userRepository = new Mock<IUserRepository<User, string>>();
            _roleRepository = new Mock<IRoleRepository<IdentityRole, string>>();
            _permissionManagerFacade = new Mock<IPermissionManagerFacade<IdentityPermission>>();
        }

        [OneTimeTearDown]
        public void Dispose()
        {
        }
    }
}
