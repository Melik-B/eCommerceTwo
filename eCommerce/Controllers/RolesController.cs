using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            var result = _roleService.GetRoles();
            ViewBag.Result = result.Message;
            return View(result.Data);
        }

        // GET: Roller/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("Error", "Id is required!");
            }
            var result = _roleService.GetRole(id.Value);
            if (!result.IsSuccessful)
            {
                ViewBag.Sonuc = result.Message;
            }
            return View(result.Data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoleModel rol)
        {
            ModelState.Remove(nameof(rol.Id));
            if (ModelState.IsValid)
            {
                var result = _roleService.Add(rol);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            return View(rol);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Error", "Id is required!");
            }
            var result = _roleService.GetRole(id.Value);
            if (!result.IsSuccessful)
            {
                return View("Error", result.Message);
            }
            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RoleModel rol)
        {
            if (ModelState.IsValid)
            {
                var result = _roleService.Update(rol);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            return View(rol);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("Error", "Id is required!");
            }
            var result = _roleService.Delete(id.Value);
            TempData["Sonuc"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
