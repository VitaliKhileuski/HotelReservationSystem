using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : Controller
    {
        private readonly LocationsService _locationsService;
        public LocationsController(LocationsService locationsService)
        {
            _locationsService = locationsService;
        }

        [HttpGet]
        [Route("countries")]
        public IActionResult GetUniqueCountries()
        {
            var countries = _locationsService.GetCountries();
            return Ok(countries);
        }
        [HttpGet]
        [Route("cities/{country}")]
        public IActionResult GetUniqueCities(string country)
        {
            var cities = _locationsService.GetCities(country);
            return Ok(cities);
        }
    }
}