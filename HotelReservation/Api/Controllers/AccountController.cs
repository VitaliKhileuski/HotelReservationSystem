using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Exceptions;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAuthenticationService authService;
        public AccountController(IAuthenticationService authService)
        {
            this.authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestModel user)
        {
            try
            {
                return Ok(await authService.Login(user));
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
                return Ok(await authService.Registration(user));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("ok");
        }
    }
}
