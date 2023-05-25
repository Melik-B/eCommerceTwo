using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    [Route("[controller]")]
    public class CitiesAjaxController : Controller
    {
        private readonly ICityService _cityService;

        public CitiesAjaxController(ICityService cityService)
        {
            _cityService = cityService;
        }


        [Route("CitiesGet/{coujntryId?}")]
        public IActionResult CitiesGet(int? countryId)
        {
            if (!countryId.HasValue)
                return NotFound();


            var result = _cityService.GetCities(countryId.Value);

            return Json(result.Data);
        }

        [Route("CitiesPost/{countryId?}")] // ~/citiesAjax/citiesPost
        [HttpPost]
        public IActionResult CitiesPost(int? countryId)
        {
            if (!countryId.HasValue)
                return NotFound();
            var result = _cityService.GetCities(countryId.Value);
            return Json(result.Data);
        }
    }
}
