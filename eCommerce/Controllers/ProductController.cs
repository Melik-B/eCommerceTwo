using Business.Models;
using Business.Services;
using Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eCommerce.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(_productService.Query().ToList());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return View("Error", "Id is required!");
            ProductModel model = _productService.Query().SingleOrDefault(u => u.Id == id.Value);
            if (model == null)
                return View("Error", "Record not found!");
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            ProductModel model = new ProductModel()
            {
                ExpirationDate = DateTime.Today,
                UnitPrice = 0,
                StockQuantity = 0
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                var result = _productService.Add(product);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View("Error", "Id is required!");
            ProductModel model = _productService.Query().SingleOrDefault(u => u.Id == id.Value);
            if (model == null)
                return View("Hata", "Record not found!");
            ViewBag.CategoryId = new SelectList(_categoryService.Query().ToList(), "Id", "Adi", model.CategoryId);

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _productService.Update(model);
                if (result.IsSuccessful)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewBag.CategoryId = new SelectList(_categoryService.Query().ToList(), "Id", "Name", model.CategoryId);

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return View("Error", "Id is required!");
            ProductModel model = _productService.Query().SingleOrDefault(u => u.Id == id.Value);
            if (model == null)
                return View("Error", "Record not found!");
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _productService.Delete(id);

            ProductModel model = new ProductModel()
            {
                Id = id,
            };

            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}