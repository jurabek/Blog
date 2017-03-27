using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Blog.Abstractions.Fasades;
using Moq;
using Blog.Web.Tests.IoC;
using Blog.Web.Controllers;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;
using Blog.Model.ViewModels;
using Blog.Model;

namespace Blog.Web.Tests.Controllers
{
    public class AccountControllerTest : BaseControllerTest
    {
        public Mock<IUserManagerFacade> UserManagerFacade { get; set; }

        public Mock<ISignInManagerFacade> SignInManagerFacade { get; set; }

        public override void Init()
        {
            Container = DependencyInjection.Initialize();
            UserManagerFacade = new Mock<IUserManagerFacade>();
            SignInManagerFacade = new Mock<ISignInManagerFacade>();
        }
        

        public class A
        {
            public Permission Permission { get; set; }
        }


        [Test]
        public void SampleTest()
        {
            var a = new A { Permission = Permission.CreateArticle | Permission.CreateComment | Permission.DeleteComment };

            int val = (int)a.Permission;

            var enval = (Permission)val;
            
            
        }

        


        [Test]
        public void LoginActionTest()
        {
            var accountController = new AccountController(UserManagerFacade.Object, SignInManagerFacade.Object, null);

            var result = accountController.Login(string.Empty) as ViewResult;

            Assert.IsNotNull(result);

            Assert.IsInstanceOf<ActionResult>(result);
        }

        [Test]
        public async Task LoginShouldBeSuccesWhenEnteredValidLoginAndPassword()
        {
            SignInManagerFacade.Setup(sm => sm.PasswordSignInAsync("valid", "valid", false))
                .Returns(Task.Run(() => SignInStatus.Success));

            var accountController = new AccountController(UserManagerFacade.Object, SignInManagerFacade.Object, null);

            var result = await accountController.Login(new LoginViewModel
            {
                Email = "valid",
                Password = "valid",
                RememberMe = false
            }, null) as RedirectToRouteResult;


            Assert.IsTrue(accountController.ModelState.IsValid);


            Assert.IsInstanceOf<RedirectToRouteResult>(result);


            Assert.AreEqual("Home",
                            result.RouteValues["controller"],
                            "Controller should be Home after logining it will rederict into Home controller");

            Assert.AreEqual("Index",
                result.RouteValues["action"],
                "Expected action shoulde be Index beacause if login success it rederects into Index");

        }
    }
}
