using System;
using HotelReservation.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
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
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Internal;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UsersService _usersService;
        private readonly IAuthenticationService _authService;
        private readonly Mapper _mapper;

        public UserController(UsersService usersService, IAuthenticationService authService,CustomMapperConfiguration cfg)
        {
            _mapper = new Mapper(cfg.UsersConfiguration);
            _usersService = usersService;
            _authService = authService;
        }
        [HttpGet]
        public   IEnumerable<UserResponseViewModel> Get()
        {
            var responseUsers = _mapper.Map<List<UserResponseViewModel>>(_usersService.GetAll());

           return responseUsers;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<UserModel> GetById(int id)
        {
            try
            {
                var responseUser = _mapper.Map<UserModel,UserResponseViewModel>(_usersService.GetById(id));

                return Ok(responseUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] RegisterUserRequestModel user)
        {
            try
            {
                var registerModel = _mapper.Map<RegisterUserRequestModel,RegisterUserModel>(user);
                return Ok(await _authService.Registration(registerModel));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _usersService.DeleteById(id);
                return Ok($"user with id {id} deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut]
        [Route("{id:int}")]
        public IActionResult Update(int id, [FromBody] UserResponseViewModel user)
        {
            try
            {
                var userModel = _mapper.Map<UserResponseViewModel, UserModel>(user);
                _usersService.Update(id, userModel);
                return Ok($"user with id {id} updated successfully");
            }
            catch (NotFoundException ex)
            {
              return  BadRequest(ex.Message);
            }
        }


    }
}