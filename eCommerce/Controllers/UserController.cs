using Business.Models;
using Business.Services;
using DataAccess.Contexts;
using DataAccess.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eCommerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly eCommerceContext _context;

        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;

        public UserController(IUserService userService, IRoleService roleService, ICountryService countryService, ICityService cityService)
        {
            _userService = userService;
            _roleService = roleService;
            _countryService = countryService;
            _cityService = cityService;
        }

        public IActionResult Index()
        {
            var result = _userService.GetUsers();
            ViewBag.Sonuc = result.Message;
            return View(result.Data);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("Hata", "Id is required!");
            }
            var result = _userService.GetUser(id.Value);
            if (!result.IsSuccessful)
            {
                ViewBag.reuslt = result.Message;
            }
            return View(result.Data);
        }

        public IActionResult Create()
        {
            var roleResult = _roleService.GetRoles();
            var countryResult = _countryService.GetCountries();
            ViewData["RoleId"] = new SelectList(roleResult.Data, "Id", "Name");
            ViewData["CountryId"] = new SelectList(countryResult.Data, "Id", "Name");
            UserModel model = new UserModel()
            {
                IsActive = true,
                UserDetails = new UserDetailModel()
                {
                    Gender = Gender.Female
                }
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserModel user)
        {
            ModelState.Remove(nameof(user.Id));
            if (ModelState.IsValid)
            {
                var result = _userService.Add(user);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            var roleResult = _roleService.GetRoles();
            var countryResult = _countryService.GetCountries();
            var cityResult = _cityService.GetCities(user.UserDetails.CountryId ?? -1);
            ViewData["RoleId"] = new SelectList(roleResult.Data, "Id", "Name", user.RoleId);
            ViewData["CountryId"] = new SelectList(countryResult.Data, "Id", "Name", user.UserDetails.CountryId ?? -1);
            ViewData["CityId"] = new SelectList(cityResult.Data, "Id", "Name", user.UserDetails.CityId ?? -1);
            return View(user);
        }

        // GET: Kullanicilar/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Error", "Id is required!");
            }
            var result = _userService.GetUser(id.Value);
            if (!result.IsSuccessful)
            {
                return View("Error", result.Message);
            }
            var roleResult = _roleService.GetRoles();
            var countryResult = _countryService.GetCountries();
            var cityResult = _cityService.GetCities(result.Data.UserDetails.CountryId ?? -1);
            ViewData["RoleId"] = new SelectList(roleResult.Data, "Id", "Name", result.Data.RoleId);
            ViewData["CountryId"] = new SelectList(countryResult.Data, "Id", "Name", result.Data.UserDetails.CountryId ?? -1);
            ViewData["CityId"] = new SelectList(cityResult.Data, "Id", "Name", result.Data.UserDetails.CityId ?? -1);
            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.Update(user);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            var roleResult = _roleService.GetRoles();
            var countryResult = _countryService.GetCountries();
            var cityResult = _cityService.GetCities(user.UserDetails.CountryId ?? -1);
            ViewData["RoleId"] = new SelectList(roleResult.Data, "Id", "Name", user.RoleId);
            ViewData["CountryId"] = new SelectList(countryResult.Data, "Id", "Name", user.UserDetails.CountryId ?? -1);
            ViewData["CityId"] = new SelectList(cityResult.Data, "Id", "Name", user.UserDetails.CityId ?? -1);
            return View(user);
        }

        // GET: Kullanicilar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var result = _userService.Delete(id);

            UserModel model = new UserModel()
            {
                Id = id,
            };

            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}