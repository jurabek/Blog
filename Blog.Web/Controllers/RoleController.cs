using Blog.Abstractions.Managers;
using Blog.Abstractions.Repositories;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using Blog.Abstractions.Facades;
using IdentityPermissionExtension;

namespace Blog.Web.Controllers
{
    [Authorize(Roles = nameof(Roles.Administrator))]
    public class RoleController : BaseController
    {
        private readonly IRepository<IdentityRole, string, IdentityResult> _roleRepository;
        private readonly IMappingManager _mappingManager;

        public RoleController(IRepository<IdentityRole, string, IdentityResult> roleRepository, 
            IMappingManager mappingManager,
            IUrlHelperFacade urlHelper) : base(urlHelper)
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
                var result = _roleRepository.Add(_mappingManager.Map<RoleViewModel, IdentityRole>(model));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }
            return View();
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
                var result = _roleRepository.Update(_roleRepository.Get(model.Id));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                AddErrors(result);
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
            var result = _roleRepository.Delete(model);
            if (result.Succeeded)
                return RedirectToAction("Index");

            return View();

        }
    }
}
