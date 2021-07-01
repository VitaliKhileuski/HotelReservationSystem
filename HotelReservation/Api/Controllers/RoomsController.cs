using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using HotelReservation.Api.Helpers;
using HotelReservation.Api.Mappers;
using HotelReservation.Api.Models.RequestModels;
using HotelReservation.Api.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using HotelReservation.Api.Policy;
using System;

namespace HotelReservation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : Controller
    {
        private readonly Mapper _mapper;
        private readonly IRoomService _roomsService;

        public RoomsController(CustomMapperConfiguration cfg, IRoomService roomsService)
        {
            _roomsService = roomsService;
            _mapper = new Mapper(cfg.RoomConfiguration);
        }

        // [HttpGet]
        // [Route("{hotelId}")]
        // public async Task<IActionResult> GetRooms(Guid hotelId, DateTime checkInDate, DateTime checkOutDate)
        // {
        //     var roomModels = await _roomsService.GetRoomsFromHotel(hotelId,checkInDate,checkOutDate);
        //     var roomResponseModels = _mapper.Map<ICollection<RoomResponseModel>>(roomModels);
        //     return Ok(roomResponseModels);
        // }

        [HttpGet]
        [Route("{hotelId}")]
        public async Task<IActionResult> GetPage(Guid hotelId,DateTime checkInDate,DateTime checkOutDate, [FromQuery] Pagination filter)
        {
            var validFilter = new Pagination(filter.PageNumber, filter.PageSize);
            var pageInfo = await _roomsService.GetRoomsPage(hotelId,checkInDate,checkOutDate,validFilter);
            var rooms = _mapper.Map<List<RoomResponseModel>>(pageInfo.Items);
            var responsePageInfo = new PageInfo<RoomResponseModel>
            {
                Items = rooms, NumberOfItems = pageInfo.NumberOfItems, NumberOfPages = pageInfo.NumberOfPages
            };

            return Ok(responsePageInfo);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        [Route("{hotelId}")]
        public async Task<IActionResult> CreateRoom(Guid hotelId,RoomRequestModel room)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            if (room == null)
            {
                return BadRequest("incorrect input data");
            }

            var roomModel = _mapper.Map<RoomRequestModel, RoomModel>(room);
            await _roomsService.AddRoom(hotelId, roomModel, userId);
            return Ok("Added successfully");
            
        }

        [HttpDelete]
        [Route("{roomId}")]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        public async Task<IActionResult> DeleteRoom(Guid roomId)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            await _roomsService.DeleteRoom(roomId, userId);
            return Ok("Deleted Successfully");
        }

        [HttpPut]
        [Route("{roomId}")]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        public async Task<IActionResult> UpdateRoom(Guid roomId,[FromBody] RoomRequestModel room)
        {
            var roomModel = _mapper.Map<RoomRequestModel, RoomModel>(room);
            var userId = TokenData.GetIdFromClaims(User.Claims);
            await _roomsService.UpdateRoom(roomId, userId, roomModel);
            return Ok("Updated Successfully");
        }
    }
}