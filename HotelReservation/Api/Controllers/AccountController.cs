using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Models;
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
        private readonly Mapper _mapper;

        public AccountController(IAuthenticationService authService,CustomMapperConfiguration cfg)
        {
            _mapper = new Mapper(cfg.UsersConfiguration);
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestModel user)
        {
            try
            {
                var loginModel = _mapper.Map<LoginUserRequestModel,LoginUserModel>(user);
                return Ok(await _authService.Login(loginModel));
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (IncorrectPasswordException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestModel user)
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
        [Authorize(Policy = "ManageHotelsPermission")]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("ok");
        }
    }
}