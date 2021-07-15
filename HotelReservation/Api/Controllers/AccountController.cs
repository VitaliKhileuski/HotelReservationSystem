using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Models;
using HotelReservation.Api.Helpers;
using HotelReservation.Api.Mappers;
using HotelReservation.Api.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authService;
        private readonly Mapper _userMapper;
        private readonly Mapper _tokenMapper;

        public AccountController(IAuthenticationService authService,CustomMapperConfiguration cfg)
        {
            _userMapper = new Mapper(cfg.UsersConfiguration);
            _tokenMapper = new Mapper(cfg.TokenConfiguration);
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestModel user)
        {
            var loginModel = _userMapper.Map<LoginUserRequestModel,LoginUserModel>(user);
            return Ok(await _authService.Login(loginModel));    
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestModel user)
        {
            var registerModel = _userMapper.Map<RegisterUserRequestModel,RegisterUserModel>(user);
            return Ok(await _authService.Registration(registerModel));
        }

        [HttpPut("refreshTokenVerification")]
        public async Task<IActionResult> RefreshTokenVerification([FromBody] RefreshTokenRequestModel refreshToken)
        {
            var tokenModel = _tokenMapper.Map<RefreshTokenRequestModel, TokenModel>(refreshToken);
            var result = await _authService.RefreshTokenVerification(tokenModel);
            return Ok(result);
        }
        [HttpGet("tokenVerification")]
        [Authorize]
        public IActionResult TokenVerification()
        {
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("checkPassword")]
        public async Task<IActionResult> CheckPassword([FromBody] LoginUserRequestModel user)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            var result = await  _authService.IsPasswordCorrect(userId,user.Password);
            return Ok(result);

        }
    }
}