using System;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models.RequestModels;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : Controller
    {
        private readonly IHotelsService _hotelsService;
        public HotelController(IHotelsService  hotelService)
        {
            _hotelsService = hotelService;
        }
        [Authorize(Policy = "AdminPermission")]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("ok");
        }

        [HttpPost]
        [Authorize(Policy = "AdminPermission")]
        public async Task<IActionResult> AddHotel([FromBody] HotelRequestModel hotel)
        {
            try
            {
                await  _hotelsService.AddHotel(hotel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}