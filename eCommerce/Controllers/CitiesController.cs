using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eCommerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CitiesController : Controller
    {
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;

        public CitiesController(ICityService cityService, ICountryService countryService)
        {
            _cityService = cityService;
            _countryService = countryService;
        }

        public IActionResult Index()
        {
            return View(_cityService.Query().ToList());
        }

        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_countryService.Query().ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CityModel city)
        {
            if (ModelState.IsValid)
            {
                var result = _cityService.Add(city);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            ViewData["CountryId"] = new SelectList(_countryService.Query().ToList(), "Id", "Name", city.CountryId);
            return View(city);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Hata", "Id gereklidir!");
            }
            CityModel city = _cityService.Query().SingleOrDefault(s => s.Id == id.Value);
            if (city == null)
            {
                return View("Hata", "Kayıt bulunamadı!");
            }
            ViewData["CountryId"] = new SelectList(_countryService.Query().ToList(), "Id", "Name", city.CountryId);
            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CityModel city)
        {
            if (ModelState.IsValid)
            {
                var result = _cityService.Update(city);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            ViewData["CountryId"] = new SelectList(_countryService.Query().ToList(), "Id", "Name", city.CountryId);
            return View(city);
        }

        // GET: cities/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("Hata", "Id gereklidir!");
            }
            var result = _cityService.Delete(id.Value);
            TempData["Result"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
