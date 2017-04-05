using Blog.Abstractions.Facades;
using Blog.Abstractions.Managers;
using Blog.Core.Managers;
using Blog.Model.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Web.Tests.Managers
{
    [TestFixture]
    public class EmailManagerTest
    {
        Mock<IUserManagerFacade<User>> userManagerFacade;
        Mock<IUrlHelperFacade> urlHelperFacade;
        IEmailManager emailManager;

        [OneTimeSetUp]
        public void Init()
        {
            userManagerFacade = new Mock<IUserManagerFacade<User>>();
            urlHelperFacade = new Mock<IUrlHelperFacade>();
            emailManager = new EmailManager(userManagerFacade.Object, urlHelperFacade.Object);

            userManagerFacade.Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<string>()))
               .Returns(Task.FromResult("test"));

            urlHelperFacade.Setup(x => x.Action(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .Returns("test");
        }
        [Test]
        public void CanSendConfirmationEmail()
        {            
            var task = Task.Run(() => 
            {
                // here email will be send!
            });
            userManagerFacade.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(task);

            var result = emailManager.SendConfirmationEmail("");
            Assert.IsInstanceOf<Task>(result);
        }


        [Test]
        public void CanSendResetPasswordEmail()
        {
            var task = Task.Run(() =>
            {
                // here email will be send!
            });
            userManagerFacade.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(task);

            var result = emailManager.SendResetPasswordEmail("some userId");

            Assert.IsInstanceOf<Task>(result);
        }
    }
}
