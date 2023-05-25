using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize]
        public IActionResult Index()
        {
            List<CategoryModel> categories = _categoryService.Query().ToList();

            if (categories == null || categories.Count == 0)
                return View("Error", "Record not found.");

            return View("CategoryList", categories);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetCreate()
        {
            return View("CreateHtml");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult OlusturGonder(string Name, string Description)
        {
            if (string.IsNullOrWhiteSpace(Name))
                return View("Error", "Malzeme adı zorunludur!");
            if (Name.Length > 100)
                return View("Error", "Malzeme adı en fazla 100 karakter olmalıdır!");
            if (!string.IsNullOrWhiteSpace(Description) && Description.Length > 4000)
                return View("Error", "Malzeme açıklaması en fazla 4000 karakter olmalıdır!");

            CategoryModel model = new CategoryModel()
            {
                Name = Name
            };

            Result result = _categoryService.Add(model);
            if (result.IsSuccessful)
            {

                return RedirectToAction(nameof(Index));

            }
            return View("Error", result.Message);
        }

        //https://httpstatuses.com/
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return View("Error", "Id is required!");
            }

            CategoryModel model = _categoryService.Query().SingleOrDefault(k => k.Id == id.Value);

            if (model == null)
            {
                return View("Error", "Record not found!");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _categoryService.Update(model);
                if (result.IsSuccessful)
                {
                    TempData["Success"] = result.Message;

                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = result.Message;
            }

            return View(model);
        }

        public IActionResult Delete(int? id)
        {
            if (!(User.Identity.IsAuthenticated && User.IsInRole("Admin")))
                return RedirectToAction("Giris", "Hesaplar");

            if (!id.HasValue)
                return View("Error", "Id is required!");
            var result = _categoryService.Delete(id.Value);
            if (result.IsSuccessful)
            {
                TempData["Success"] = result.Message;
            }
            else
            {
                TempData["Error"] = result.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
                return View("Error", "Id is required!");
            CategoryModel model = _categoryService.Query().SingleOrDefault(k => k.Id == id.Value);
            if (model == null)
                return View("Error", "Record not found!");
            return View(model);
        }
    }
}