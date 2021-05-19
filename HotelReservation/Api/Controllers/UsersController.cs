﻿using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Models;
using HotelReservation.Api.Mappers;
using HotelReservation.Api.Models.RequestModels;
using HotelReservation.Api.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _usersService;
        private readonly IAuthenticationService _authService;
        private readonly Mapper _mapper;

        public UsersController(IUserService usersService, IAuthenticationService authService,CustomMapperConfiguration cfg)
        {
            _mapper = new Mapper(cfg.UsersConfiguration);
            _usersService = usersService;
            _authService = authService;
        }
        [HttpGet]
        [Authorize(Policy = "AdminPermission")]
        public IEnumerable<UserResponseViewModel> Get()
        {
            var responseUsers = _mapper.Map<List<UserResponseViewModel>>(_usersService.GetAll());
           return responseUsers;
        }

        [HttpGet]
        [Authorize(Policy = "AdminPermission")]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var responseUser = _mapper.Map<UserModel, UserResponseViewModel>(await _usersService.GetById(id));
            return Ok(responseUser);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPermission")]
        public async Task<IActionResult> Add([FromBody] RegisterUserRequestModel user)
        {
            var registerModel = _mapper.Map<RegisterUserRequestModel,RegisterUserModel>(user);
            return Ok(await _authService.Registration(registerModel));
        }

        [HttpDelete]
        [Authorize(Policy = "AdminPermission")]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _usersService.DeleteById(id);
            return Ok($"user with id {id} deleted successfully");
        }

        [HttpPut]
        [Authorize]
        [Route("{id:int}")]
        public IActionResult Update(int id, [FromBody] UserResponseViewModel user)
        {
            var userModel = _mapper.Map<UserResponseViewModel, UserModel>(user);
            _usersService.Update(id, userModel);
            return Ok($"user with id {id} updated successfully");
        }
    }
}