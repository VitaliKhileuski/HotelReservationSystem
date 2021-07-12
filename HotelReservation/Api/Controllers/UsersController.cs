﻿using Microsoft.AspNetCore.Mvc;
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
using System.Collections;
using HotelReservation.Api.Helpers;
using HotelReservation.Data.Entities;

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
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            var userModel = await _usersService.GetById(id, userId);
            var responseUser = _mapper.Map<UserModel, UserResponseViewModel>(userModel);
            return Ok(responseUser);
        }

        [HttpGet]
        [Authorize(Policy = Policies.AdminPermission)]
        public async Task<IActionResult> GetUsersPage([FromQuery] Pagination pagination)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            var pageInfo = await _usersService.GetUsersPage(userId, pagination);
            var userResponseModels = _mapper.Map <ICollection<UserResponseViewModel>>(pageInfo.Items);
            var page = new PageInfo<UserResponseViewModel>
            {
                Items = userResponseModels,
                NumberOfItems = pageInfo.NumberOfItems,
                NumberOfPages = pageInfo.NumberOfPages
            };
            return Ok(page);
        }


        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserRequestModel user)
        {
            var userModel = _mapper.Map<UserRequestModel, UserModel>(user);
            await _usersService.AddUser(userModel);
            return Ok("added successfully");
        }

        [HttpDelete]
        [Authorize(Policy = Policies.AdminPermission)]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _usersService.DeleteById(id);
            return Ok($"user with id {id} deleted successfully");
        }

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserRequestModel user)
        {

            var userId = TokenData.GetIdFromClaims(User.Claims);
            var userModel = _mapper.Map<UserRequestModel, UserModel>(user);
            var token = await _usersService.Update(id, userId, userModel);
            var tokenModel = new TokenModel
            {
                Token = token
            };
            return Ok(tokenModel);
        }
    }
}