using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Models;
using HotelReservation.Api.Mappers;
using HotelReservation.Api.Models.RequestModels;
using HotelReservation.Api.Models.ResponseModels;
using HotelReservation.Api.Policy;
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

        public HotelsController(IHotelsService hotelService, CustomMapperConfiguration cfg)
        {
            _mapper = new Mapper(cfg.HotelConfiguration);
            _hotelsService = hotelService;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<HotelResponseModel> GetByiD(int id)
        {
            var responseHotel = _mapper.Map<HotelModel, HotelResponseModel>(await _hotelsService.GetById(id));
            return responseHotel;
        }

        [HttpGet]
        [Route("pages")]
        public IActionResult GetPage([FromQuery] HotelPagination filter)
        { 
            var validFilter = new HotelPagination(filter.PageNumber, filter.PageSize);
            var  HotelsWithCount =_hotelsService.GetHotelsPage(validFilter);
            var hotels = _mapper.Map<List<HotelResponseModel>>(HotelsWithCount.Item1);
            var maxNumberOfHotels = HotelsWithCount.Item2;

            return Ok(Tuple.Create(hotels,maxNumberOfHotels));
        }

        [HttpGet]
        public IActionResult GetFilteredGHotels(DateTime checkInDate, DateTime checkOutDate, string country,
            string city, [FromQuery] HotelPagination filter)
        {
            var validFilter = new HotelPagination(filter.PageNumber, filter.PageSize);
            return Ok(_hotelsService.GetFilteredHotels(checkInDate, checkOutDate, country, city,validFilter));
        }


        [HttpPost]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        public async Task<IActionResult> AddHotel([FromBody] HotelModel hotel)
        {
            await  _hotelsService.AddHotel(hotel);
            return Ok("added successfully");
        }

        [HttpPut]
        [Authorize(Policy = "AdminPermission")]
        [Route("{hotelId:int}/setHotelAdmin")]
        public IActionResult UpdateHotelAdmin(int hotelId, [FromBody] int userId)
        {
            _hotelsService.UpdateHotelAdmin(hotelId, userId);
            return Ok("admin setted successfully");
        }

        [HttpPut]
        [Authorize(Policy = "HotelAdminPermission")]
        [Route("{id:int}/editHotel")]
        public async Task<IActionResult> EditHotel(int id,[FromBody] HotelRequestModel hotel)
        {
            var idClaim = int.Parse(User.Claims.FirstOrDefault(x =>
                x.Type.ToString().Equals("id", StringComparison.InvariantCultureIgnoreCase))
                ?.Value ?? string.Empty);
            var hotelModel = _mapper.Map<HotelRequestModel, HotelModel>(hotel);
            await _hotelsService.UpdateHotel(id, hotelModel,idClaim);
            return Ok("Updated Successfully");
        }

        [HttpDelete]
        [Authorize(Policy = "AdminPermission")]
        [Route("{hotelId:int}/deleteHotel")]
        public async Task<IActionResult> DeleteHotelById(int hotelId)
        {
            await _hotelsService.DeleteHotelById(hotelId);
            return Ok("Deleted successfully");
        }
    }
}