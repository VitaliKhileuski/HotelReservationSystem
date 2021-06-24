using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using HotelReservation.Api.Helpers;
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
        private readonly Mapper _hotelMapper;
        private readonly Mapper _userMapper;
        private readonly IHotelsService _hotelsService;


        public HotelsController(IHotelsService hotelService, CustomMapperConfiguration cfg)
        {
            _hotelMapper = new Mapper(cfg.HotelConfiguration);
            _hotelsService = hotelService;
            _userMapper = new Mapper(cfg.UsersConfiguration);
        }

        [HttpGet]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        [Route("{id:int}")]
        public async Task<HotelResponseModel> GetByiD(int id)
        {
            var responseHotel = _hotelMapper.Map<HotelModel, HotelResponseModel>(await _hotelsService.GetById(id));
            return responseHotel;
        }

        [HttpGet]
        [Authorize(Policy = Policies.AdminPermission)]
        [Route("pages")]
        public async Task<IActionResult> GetPage([FromQuery] Pagination filter)
        {
            var validFilter = new Pagination(filter.PageNumber, filter.PageSize);
            var hotelsWithCount = await _hotelsService.GetHotelsPage(validFilter);
            var hotels = _hotelMapper.Map<List<HotelResponseModel>>(hotelsWithCount.Item1);
            var maxNumberOfHotels = hotelsWithCount.Item2;

            return Ok(Tuple.Create(hotels, maxNumberOfHotels));
        }
        [HttpGet]
        [Authorize(Policy = Policies.HotelAdminPermission)]
        [Route("hotelAdmin/{hotelAdminId:int}/pages")]
        public async Task<IActionResult> GetHotelAdminsPage([FromQuery] Pagination filter,int  hotelAdminId)
        {
            var validFilter = new Pagination(filter.PageNumber, filter.PageSize);
            var  hotelsWithCount = await _hotelsService.GetHotelAdminPages(validFilter,hotelAdminId);
            var hotels = _hotelMapper.Map<List<HotelResponseModel>>(hotelsWithCount.Item1);
            var maxNumberOfHotels = hotelsWithCount.Item2;

            return Ok(Tuple.Create(hotels, maxNumberOfHotels));
        }

        [HttpGet]
        public IActionResult GetFilteredGHotels(DateTime checkInDate, DateTime checkOutDate, string country,
            string city, [FromQuery] Pagination filter)
        {
            var validFilter = new Pagination(filter.PageNumber, filter.PageSize);
            return Ok(_hotelsService.GetFilteredHotels(checkInDate, checkOutDate, country, city, validFilter));
        }

        [HttpGet]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        [Route("{hotelId:int}/getHotelAdmins")]
        public async Task<IActionResult> GetHotelAdmins(int hotelId)
        {
            var hotelAdmins = await _hotelsService.GetHotelAdmins(hotelId);
            return Ok(_userMapper.Map<ICollection<UserResponseViewModel>>(hotelAdmins));
        }


        [HttpPost]
        [Authorize(Policy = Policies.AdminPermission)]
        public async Task<IActionResult> AddHotel([FromBody] HotelModel hotel)
        {
            await  _hotelsService.AddHotel(hotel);
            return Ok("added successfully");
        }

        [HttpPut]
        [Route("{hotelId:int}/{userId:int}/setHotelAdmin")]
        public async Task<IActionResult> UpdateHotelAdmin(int hotelId,int userId)
        {
            await _hotelsService.UpdateHotelAdmin(hotelId, userId);

            return Ok();
        }
        [HttpPut]
        [Route("{hotelId:int}/{userId:int}/deleteHotelAdmin")]
        public async Task<IActionResult> DeleteHotelAdmin(int hotelId, int userId)
        {
            await _hotelsService.DeleteHotelAdmin(hotelId, userId);

            return Ok("admin deleted successfully");
        }

        [HttpPut]
        [Authorize(Policy = Policies.AdminPermission)]
        [Route("{id:int}")]
        public async Task<IActionResult> EditHotel(int id,[FromBody] HotelRequestModel hotel)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            var hotelModel = _hotelMapper.Map<HotelRequestModel, HotelModel>(hotel);
            await _hotelsService.UpdateHotel(id, hotelModel,userId);
            return Ok("Updated Successfully");
        }

        [HttpDelete]
        [Authorize(Policy = Policies.AdminPermission)]
        [Route("{hotelId:int}")]
        public async Task<IActionResult> DeleteHotelById(int hotelId)
        {
            await _hotelsService.DeleteHotelById(hotelId);
            return Ok("Deleted successfully");
        }
    }
}