using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using HotelReservation.Api.Mappers;
using HotelReservation.Api.Models.RequestModels;
using HotelReservation.Api.Models.ResponseModels;
using HotelReservation.Api.Policy;
using Microsoft.AspNetCore.Authorization;
using System;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _usersService;
        private readonly Mapper _mapper;

        public UsersController(IUserService usersService, CustomMapperConfiguration cfg)
        {
            _mapper = new Mapper(cfg.UsersConfiguration);
            _usersService = usersService;
        }
        [HttpGet]
        [Authorize(Policy = Policies.AdminPermission)]
        [Route("{hotelId}/getPotentialHotelAdmins")]
        public async Task<IActionResult> Get(Guid hotelId)
        {
            var responseUsers = _mapper.Map<List<UserResponseViewModel>>(await _usersService.GetAll(hotelId));
           return Ok(responseUsers);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var responseUser = _mapper.Map<UserModel, UserResponseViewModel>(await _usersService.GetById(id));
            return Ok(responseUser);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AdminPermission)]
        public async Task<IActionResult> AddUser([FromBody] UserRequestModel user)
        {
            var userModel = _mapper.Map<UserRequestModel,UserModel>(user);
            await _usersService.AddUser(userModel);
            return Ok("added successfully");
        }

        [HttpDelete]
        [Authorize(Policy = "AdminPermission")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _usersService.DeleteById(id);
            return Ok($"user with id {id} deleted successfully");
        }

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public IActionResult Update(Guid id, [FromBody] UserResponseViewModel user)
        {
            var userModel = _mapper.Map<UserResponseViewModel, UserModel>(user);
            _usersService.Update(id, userModel);
            return Ok($"user with id {id} updated successfully");
        }
    }
}