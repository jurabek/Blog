using Blog.Abstractions.Managers;
using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Web.Controllers
{
    public class RoleController : Controller
    {
        private IRepository<IdentityRole, string> _roleRepository;
        private IMappingManager _mappingManager;

        public RoleController(IRepository<IdentityRole, string> roleRepository, IMappingManager mappingManager)
        {
            _roleRepository = roleRepository;
            _mappingManager = mappingManager;
        }

        public ActionResult Index()
        {
            return View(_roleRepository.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _roleRepository.Add<IdentityResult>(_mappingManager.Map<RoleViewModel, IdentityRole>(model));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public ActionResult Edit(string id)
        {
            return View(_mappingManager.Map<IdentityRole, RoleViewModel>(_roleRepository.Get(id)));
        }

        [HttpPost]
        public ActionResult Edit(RoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                var result = _roleRepository.Update<IdentityResult>(_mappingManager.Map<RoleViewModel, IdentityRole>(model));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public ActionResult Delete(string id)
        {
            return View(_roleRepository.Get(id));
        }

        [HttpPost]
        public ActionResult Delete(IdentityRole model)
        {
            var result = _roleRepository.Delete<IdentityResult>(_roleRepository.Get(model.Id));
            if (result.Succeeded)
                return RedirectToAction("Index");

            return View(model);

        }
    }
}
