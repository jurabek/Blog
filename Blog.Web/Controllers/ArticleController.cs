using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using System.IO;
using Blog.Abstractions.Managers;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using IdentityPermissionExtension;
using Blog.Model;
using Blog.Core.Repositories;

namespace Blog.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IRepository<Article, string, bool> _repository;
        private readonly IMappingManager _mappingManager;
        private IUserRepository<User, string, IdentityResult> _userRepository;

        public ArticleController(
            IRepository<Article, string, bool> repository,
            IUserRepository<User, string, IdentityResult> userRepository,
            IMappingManager mappingManager)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mappingManager = mappingManager;
        }

        public ActionResult Index()
        {
            var articles = _repository.GetAll().Take(5).ToList();
            IEnumerable<ArticleViewModel> model = _mappingManager
                .Map<IEnumerable<Article>, IEnumerable<ArticleViewModel>>(articles);
            return View(model);
        }

        public ActionResult Details(string id)
        {
            var model = _repository.Get(id);
            var viewModel = _mappingManager.Map<Article, ArticleViewModel>(model);
            return View(viewModel);
        }

        [AuthorizePermission(Roles = "Administrator", Name = nameof(Permissions.CanCreateArticle), IsGlobal = true)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AuthorizePermission(Roles = "Administrator", Name = nameof(Permissions.CanCreateArticle), IsGlobal = true)]
        public async Task<ActionResult> Create(ArticleViewModel model, HttpPostedFileBase file)
        {
            if (file != null)
            {
                model.Image = file.FileName;
                string path = Path.Combine(Server.MapPath("~/Images/Blog"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
            }

            var user = await _userRepository.GetByNameAsync(User?.Identity.GetUserName());
            model.Author = user;

            Article article = _mappingManager.Map<ArticleViewModel, Article>(model);
            if (_repository.Add(article))
                return RedirectToAction("Index");

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Rank(int rank, string articleId)
        {
            var articleRepository = _repository as ArticleRepository;
            if (!new int[] { 1, -1 }.Contains(rank))
            {
                return Json(new { Success = false, ErrorMessage = "Rank value value does not correct!" });
            }

            var user = await _userRepository.GetByNameAsync(User.Identity.GetUserName());
            var article = articleRepository.Get(articleId);

            if (article.Votes.Any(v => v.UserId == user.Id))
            {
                var vote = article.Votes.FirstOrDefault(v => v.UserId == user.Id);
                if (vote.VoteValue == rank)
                {
                    return Json(new { Success = false, ErrorMessage = "You have already ranked this article" });
                }
                else
                {
                    article.Votes.Remove(vote);
                    articleRepository.SaveChanges();
                }
            }

            article.Votes.Add(new Vote
            {
                ArticleId = articleId,
                VoteValue = rank,
                UserId = user.Id,
                VotedTime = DateTime.Now
            });
            articleRepository.SaveChanges();

            var result = article.Votes.Count(x => x.VoteValue == rank);
            return Json(new { Success = true, Result = result });

        }

        public ActionResult GetVoteDown(string articleId)
        {
            var article = _repository.Get(articleId);
            var result = article.Votes.Where(v => v.VoteValue == -1).Count();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVoteUp(string articleId)
        {
            var article = _repository.Get(articleId);
            var result = article.Votes.Where(v => v.VoteValue == 1).Count();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
