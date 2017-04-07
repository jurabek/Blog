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

namespace Blog.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IRepository<Article, string, bool> _repository;
        private readonly IMappingManager _mappingManager;

        public ArticleController(IRepository<Article, string, bool> repository,
            IMappingManager mappingManager)
        {
            _repository = repository;
            _mappingManager = mappingManager;
        }

        // GET: Article
        public ActionResult Index()
        {
            return View(_repository.GetAll());
        }

        // GET: Article/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Article/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Article/Create
        [HttpPost]
        public ActionResult Create(ArticleViewModel model, HttpPostedFileBase file)
        {
            try
            {
                if (file != null)
                {
                    model.Image = file.FileName;
                    model.DateTime = DateTime.Now;
                    string pic = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images/Blog"), pic);
                    file.SaveAs(path);
                }
                Article article = _mappingManager.Map<ArticleViewModel, Article>(model);
                var a = _repository.Add(article);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Article/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Article/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Article/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Article/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
