using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CountriesController : Controller
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        public IActionResult Index()
        {
            return View(_countryService.Query().ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CountryModel country)
        {
            if (ModelState.IsValid)
            {
                var result = _countryService.Add(country);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            return View(country);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Error", "Id is required!");
            }
            CountryModel country = _countryService.Query().SingleOrDefault(u => u.Id == id);
            if (country == null)
            {
                return View("Error", "Record not found!");
            }
            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CountryModel country)
        {
            if (ModelState.IsValid)
            {
                var result = _countryService.Update(country);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            return View(country);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("Error", "Id is required!");
            }
            var result = _countryService.Delete(id.Value);
            TempData["Result"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
