﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Models;
using HotelReservation.Api.Helpers;
using HotelReservation.Api.Mappers;
using HotelReservation.Api.Models.RequestModels;
using HotelReservation.Api.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using HotelReservation.Api.Policy;

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

        [HttpGet]
        [Route("{hotelId:int}")]
        public async Task<IActionResult> GetRooms(int hotelId)
        {
            var roomModels = await _roomsService.GetRoomsFromHotel(hotelId);
            var roomResponseModels = _mapper.Map<ICollection<RoomResponseModel>>(roomModels);
            return Ok(roomResponseModels);
        }

        [HttpGet]
        [Route("{hotelId:int}/pages")]
        public async Task<IActionResult> GetPage(int hotelId,[FromQuery] Pagination filter)
        {
            var validFilter = new Pagination(filter.PageNumber, filter.PageSize);
            var pageInfo = await _roomsService.GetRoomsPage(hotelId,validFilter);
            var rooms = _mapper.Map<List<RoomResponseModel>>(pageInfo.Items);
            var responsePageInfo = new PageInfo<RoomResponseModel>
            {
                Items = rooms, NumberOfItems = pageInfo.NumberOfItems, NumberOfPages = pageInfo.NumberOfPages
            };

            return Ok(responsePageInfo);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        [Route("{hotelId:int}")]
        public async Task<IActionResult> CreateRoom(int hotelId,RoomRequestModel room)
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
        [Route("{roomId:int}")]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        public async Task<IActionResult> DeleteRoom(int roomId)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            await _roomsService.DeleteRoom(roomId, userId);
            return Ok("Deleted Successfully");
        }

        [HttpPut]
        [Route("{roomId:int}")]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        public async Task<IActionResult> UpdateRoom(int roomId,[FromBody] RoomRequestModel room)
        {
            var roomModel = _mapper.Map<RoomRequestModel, RoomModel>(room);
            var userId = TokenData.GetIdFromClaims(User.Claims);
            await _roomsService.UpdateRoom(roomId, userId, roomModel);
            return Ok("Updated Successfully");
        }
    }
}