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
        [Route("{id}")]
        public async Task<HotelResponseModel> GetByiD(Guid id)
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
        [Route("hotelAdmin/{hotelAdminId}/pages")]
        public async Task<IActionResult> GetHotelAdminsPage([FromQuery] Pagination filter,Guid hotelAdminId)
        {
            var validFilter = new Pagination(filter.PageNumber, filter.PageSize);
            var  hotelsWithCount = await _hotelsService.GetHotelAdminPages(validFilter,hotelAdminId);
            var hotels = _hotelMapper.Map<List<HotelResponseModel>>(hotelsWithCount.Item1);
            var maxNumberOfHotels = hotelsWithCount.Item2;

            return Ok(Tuple.Create(hotels, maxNumberOfHotels));
        }

        [HttpGet]
        [Route("page")]
        public async Task<IActionResult> GetFilteredGHotels(string userId, DateTime? checkInDate, DateTime? checkOutDate, string country,
            string city,string hotelName,string email,string surname, [FromQuery] Pagination filter)
        {
            var validFilter = new Pagination(filter.PageNumber, filter.PageSize);
            var pageInfo = await  _hotelsService.GetFilteredHotels(userId, checkInDate, checkOutDate, country, city,hotelName,email,surname, validFilter);
            var hotels = _hotelMapper.Map<List<HotelResponseModel>>(pageInfo.Items);
            var responsePageInfo = new PageInfo<HotelResponseModel>
            {
                Items = hotels,
                NumberOfItems = pageInfo.NumberOfItems,
                NumberOfPages = pageInfo.NumberOfPages
            };

            return Ok(responsePageInfo);
        }

        [HttpGet]
        [Route("hotelNames")]
        public IActionResult GetHotelNames()
        {
            var hotelNames = _hotelsService.GetHotelNames();
            return Ok(hotelNames);
        }

        [HttpGet]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        [Route("{hotelId}/getRoomsNumbers")]
        public async Task<IActionResult> GetHotelRoomsNumbers(Guid hotelId)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            var roomsNumbers = await _hotelsService.GetHotelRoomsNumbers(hotelId, userId);
            return Ok(roomsNumbers);
        }

        [HttpGet]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        [Route("{hotelId}/getHotelAdmins")]
        public async Task<IActionResult> GetHotelAdmins(Guid hotelId)
        {
            var hotelAdmins = await _hotelsService.GetHotelAdmins(hotelId);
            return Ok(_userMapper.Map<ICollection<UserResponseViewModel>>(hotelAdmins));
        }


        [HttpPost]
        [Authorize(Policy = Policies.AdminPermission)]
        public async Task<IActionResult> AddHotel([FromBody] HotelModel hotel)
        {
            await  _hotelsService.AddHotel(hotel);
            return Ok();
        }

        [HttpPut]
        [Authorize(Policy = Policies.AdminPermission)]
        [Route("{hotelId}/{adminId}/setHotelAdmin")]
        public async Task<IActionResult> UpdateHotelAdmin(Guid hotelId, Guid adminId)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            await _hotelsService.UpdateHotelAdmin(hotelId, adminId);

            return Ok();
        }
        [HttpPut]
        [Authorize(Policy = Policies.AdminPermission)]
        [Route("{hotelId}/{adminId}/deleteHotelAdmin")]
        public async Task<IActionResult> DeleteHotelAdmin(Guid hotelId,Guid adminId)
        {
            await _hotelsService.DeleteHotelAdmin(hotelId, adminId);

            return Ok();
        }

        [HttpPut]
        [Authorize(Policy = Policies.AdminPermission)]
        [Route("{hotelId}")]
        public async Task<IActionResult> EditHotel(Guid hotelId,[FromBody] HotelRequestModel hotel)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            var hotelModel = _hotelMapper.Map<HotelRequestModel, HotelModel>(hotel);
            await _hotelsService.UpdateHotel(hotelId, hotelModel,userId);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Policy = Policies.AdminPermission)]
        [Route("{hotelId}")]
        public async Task<IActionResult> DeleteHotelById(Guid hotelId)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            await _hotelsService.DeleteHotelById(hotelId, userId);
            return Ok();
        }
    }
}