using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.Design;
using System.Security.Claims;
using Business.Services;
using Business.Models;

namespace eCommerce.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;

        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;

        public AccountsController(IAccountService accountService, ICountryService countryService, ICityService cityService)
        {
            _accountService = accountService;
            _countryService = countryService;
            _cityService = cityService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _accountService.Login(model);
                if (result.IsSuccessful)
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, result.Data.Username),
                        new Claim(ClaimTypes.Role, result.Data.RoleNameDisplay),
                        new Claim(ClaimTypes.Sid, result.Data.Id.ToString())
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(model);
        }

        public async Task<IActionResult> Exit()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult UnauthorizedTransaction()
        {
            return View("Hata", "You are not authorized to perform this transaction!");
        }

        public IActionResult Registration()
        {
            var result = _countryService.GetCountries();
            if (result.IsSuccessful)
                ViewBag.UlkeId = new SelectList(result.Data, "Id", "Adi");
            else
                ViewBag.Mesaj = result.Message;
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Registration(UserRegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                var registrationResult = _accountService.Register(model);
                if (registrationResult.IsSuccessful)
                    return RedirectToAction(nameof(Login));
                ViewBag.Message = registrationResult.Message;
            }
            var countryResult = _countryService.GetCountries();
            ViewBag.UlkeId = new SelectList(countryResult.Data, "Id", "Name", model.UserDetails.CityId ?? -1);
            var cityResult = _cityService.GetCities(model.UserDetails.CountryId ?? -1);
            ViewBag.SehirId = new SelectList(cityResult.Data, "Id", "Name", model.UserDetails.CityId ?? -1);
            return View(model);
        }
    }
}
