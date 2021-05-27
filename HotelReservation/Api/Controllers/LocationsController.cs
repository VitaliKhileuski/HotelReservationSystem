using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : Controller
    {
        public LocationsService _locationsService;
        public LocationsController(LocationsService locationsService)
        {
            _locationsService = locationsService;
        }

        [HttpGet]
        [Route("countries")]
        public IActionResult GetUniqueCountries()
        {
            List<string> countries = _locationsService.GetCountries();
            return Ok(countries);
        }
        [HttpGet]
        [Route("cities/{country}")]
        public IActionResult GetUniqueCities(string country)
        {
            List<string> cities = _locationsService.GetCities(country);
            return Ok(cities);
        }
    }
}
