using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Models;
using Business.Services;
using HotelReservation.Api.Mappers;
using HotelReservation.Api.Models.RequestModels;
using HotelReservation.Api.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;


namespace HotelReservation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : Controller
    {
        private readonly Mapper _mapper;
        private readonly RoomsService _roomsService;

        public RoomsController(CustomMapperConfiguration cfg, RoomsService roomsService)
        {
            _roomsService = roomsService;
            _mapper = new Mapper(cfg.RoomConfiguration);
        }

        [HttpGet]
        public async Task<IActionResult> GetRooms()
        {
            try
            {
                var rooms = _mapper.Map<List<RoomResponseModel>>(await _roomsService.GetAllRooms());
                return Ok(rooms);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{hotelId:int}")]
        public async Task<IActionResult> GetRooms(int hotelId)
        {
            try
            {
                var roomModels = await _roomsService.GetRoomsFromHotel(hotelId);
                var roomResponseModels = _mapper.Map<List<RoomResponseModel>>(roomModels);
                return Ok(roomResponseModels);
            }
            catch (NotFoundException ex)
            {
               return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Authorize(Policy = "HotelAdminPermission")]
        [Route("{hotelId:int}/addRoom")]
        public async Task<IActionResult> CreateRoom(int hotelId,RoomRequestModel room)
        {
            try
            {
                int idClaim = GetIdFromClaims();
                if (room == null)
                {
                    return BadRequest("incorrect input data");
                }

                var roomModel = _mapper.Map<RoomRequestModel, RoomModel>(room);
                await _roomsService.AddRoom(hotelId, roomModel, idClaim);
                return Ok("Added successfully");
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{hotelId:int}/{roomId:int}")]
        [Authorize(Policy = "HotelAdminPermission")]
        public async Task<IActionResult> DeleteRoom(int roomId)
        {
            try
            {
                int userId = GetIdFromClaims();
                await _roomsService.DeleteRoom(roomId, userId);
                return Ok("Deleted Successfully");
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("/{roomId:int}")]
        [Authorize(Policy = "HotelAdminPermission")]
        public async Task<IActionResult> UpdateRoom(int roomId,[FromBody] RoomRequestModel room)
        {
            try
            {
                var roomModel = _mapper.Map<RoomRequestModel, RoomModel>(room);
                int userId = GetIdFromClaims();
                await _roomsService.UpdateRoom(roomId, userId, roomModel);
                return Ok("Updated Successfully");
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private int GetIdFromClaims()
        {
            int idClaim = int.Parse(User.Claims.FirstOrDefault(x =>
                    x.Type.ToString().Equals("id", StringComparison.InvariantCultureIgnoreCase))
                ?.Value ?? string.Empty);
            return idClaim;
        }
    }
}
