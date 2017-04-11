using Blog.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using Moq;
using Microsoft.AspNet.Identity;
using Blog.Abstractions.Managers;
using Blog.Model.ViewModels;
using System.Web.Mvc;

namespace Blog.Web.Tests.Controllers
{
    [TestFixture]
    public class ArticleControllerTest : BaseControllerTest<ArticleController>
    {
        private Mock<IRepository<Article, string, bool>> _repository;
        private Mock<IUserRepository<User, string, IdentityResult>> _userRepository;
        private Mock<IMappingManager> _mappingManager;

        public override void Init()
        {
            _repository = new Mock<IRepository<Article, string, bool>>();
            _userRepository = new Mock<IUserRepository<User, string, IdentityResult>>();
            _mappingManager = new Mock<IMappingManager>();
            _controller = new ArticleController(_repository.Object, _userRepository.Object, _mappingManager.Object);
        }

        [Test]
        public void Index_should_return_viewModel()
        {
            //given

            _repository.Setup(x => x.GetAll())
                .Returns(Enumerable.Repeat(new Article(), 5));

            var model = Enumerable.Repeat(new ArticleViewModel(), 5);
            
            _mappingManager.Setup(x => x.Map<IEnumerable<Article>, IEnumerable<ArticleViewModel>>(It.IsAny<IEnumerable<Article>>()))
                .Returns(model);

            // when
            var result = _controller.Index() as ViewResult;
            
            //then
            Assert.That(result.ViewData.Model, Is.EqualTo(model));
        }

        [Test]
        public void Detailes_should_return_article_from_id()
        {
            //given
            var article = new Article();
            _repository.Setup(x => x.Get(It.IsAny<string>()))
                .Returns(article);

            // given
            var viewModel = new ArticleViewModel();
            _mappingManager.Setup(x => x.Map<Article, ArticleViewModel>(article))
                .Returns(viewModel);
            
            // when
            var result = _controller.Details("") as ViewResult;

            //then
            Assert.That(result.ViewData.Model, Is.EqualTo(viewModel));
        }

        [Test]
        public void Create_should_return_view()
        {
            // when 
            var result = _controller.Create();

            // then
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Create_should_navigate_to_index()
        {
            _mappingManager.Setup(x => x.Map<ArticleViewModel, Article>(It.IsAny<ArticleViewModel>()))
                .Returns(new Article());

            _repository.Setup(x => x.Add(It.IsAny<Article>()))
                .Returns(true);

            var result = await _controller.Create(new ArticleViewModel(), null) as RedirectToRouteResult;
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }
    }
}
