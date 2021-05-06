using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using HotelReservation.Api.Mappers;
using HotelReservation.Api.Models.RequestModels;
using HotelReservation.Api.Models.ResponseModels;
using HotelReservation.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : Controller
    {
        private readonly Mapper _mapper;
        private readonly IHotelsService _hotelsService;
        public HotelController(IHotelsService  hotelService,CustomMapperConfiguration cfg)
        {
            _mapper = new Mapper(cfg.HotelConfiguration);
            _hotelsService = hotelService;
        }
        [Authorize(Policy = "HotelAdminPermission")]
        [HttpGet("test")]
        public IActionResult Test()
        {
            var idClaim = int.Parse(User.Claims.FirstOrDefault(x =>
                    x.Type.ToString().Equals("id", StringComparison.InvariantCultureIgnoreCase))
                ?.Value ?? string.Empty);
            return Ok("ok");
        }

        [HttpGet]
        public ICollection<HotelResponseModel> GetAll()
        {
            var hotelResponseModels = _mapper.Map<List<HotelResponseModel>>(_hotelsService.GetAll());
            return hotelResponseModels;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<HotelResponseModel> GetByiD(int id)
        {
            var responseHotel = _mapper.Map<HotelModel,HotelResponseModel>(await _hotelsService.GetById(id));
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

        [HttpPut]
        [Authorize(Policy = "AdminPermission")]
        [Route("hotelAdmin/{id:int}")]
        public void UpdateHotelAdmin(int id, [FromBody] int userId)
        {
            _hotelsService.UpdateHotelAdmin(id, userId);
        }
    }
}