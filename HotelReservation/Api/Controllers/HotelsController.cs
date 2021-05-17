using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
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
    public class HotelsController : Controller
    {
        private readonly Mapper _mapper;
        private readonly IHotelsService _hotelsService;
        public HotelsController(IHotelsService hotelService,CustomMapperConfiguration cfg)
        {
            _mapper = new Mapper(cfg.HotelConfiguration);
            _hotelsService = hotelService;
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
        [Authorize(Policy = "AdminPermission")]
        public async Task<IActionResult> AddHotel([FromBody] HotelModel hotel)
        {
            try
            {
                await  _hotelsService.AddHotel(hotel);
                return Ok("added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Policy = "AdminPermission")]
        [Route("{hotelId:int}/setHotelAdmin")]
        public IActionResult UpdateHotelAdmin(int hotelId, [FromBody] int userId)
        {
            try
            {
                _hotelsService.UpdateHotelAdmin(hotelId, userId);
                return Ok("admin setted successfully");
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut]
        [Authorize(Policy = "HotelAdminPermission")]
        [Route("{id:int}/editHotel")]
        public async Task<IActionResult> EditHotel(int id,[FromBody] HotelRequestModel hotel)
        {
            try
            {
                if (hotel.Name == null && hotel.Location == null)
                {
                    return BadRequest("incorrect input data");
                }
                var idClaim = int.Parse(User.Claims.FirstOrDefault(x =>
                        x.Type.ToString().Equals("id", StringComparison.InvariantCultureIgnoreCase))
                    ?.Value ?? string.Empty);
                var hotelModel = _mapper.Map<HotelRequestModel, HotelModel>(hotel);
                await _hotelsService.UpdateHotel(id, hotelModel,idClaim);
                return Ok("Updated Successfully");
            }
            catch (BadRequestException ex)
            {
              return  BadRequest(ex.Message);
            }
        }
    }
}