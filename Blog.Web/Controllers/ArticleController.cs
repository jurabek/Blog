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
            IEnumerable<ArticleViewModel> model = _mappingManager
                .Map<IEnumerable<Article>, IEnumerable<ArticleViewModel>>(_repository.GetAll().Take(5).ToList());
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
