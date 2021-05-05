using System;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using HotelReservation.Api.Mappers;
using HotelReservation.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : Controller
    {
        private readonly RequestMapper _mapper = new RequestMapper();
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

        [HttpGet]
        [Route("{id:int}")]
        public async Task<HotelResponseModel> GetByiD(int id)
        {
            var responseHotel = _mapper.MapItem<HotelModel,HotelResponseModel>(await _hotelsService.GetById(id));
            return responseHotel;
        }

        [HttpPost]
        //[Authorize(Policy = "AdminPermission")]
        public async Task<IActionResult> AddHotel([FromBody] HotelModel hotel)
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