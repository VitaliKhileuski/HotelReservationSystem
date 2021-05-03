using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Business.Exceptions;
using Business.Interfaces;
using Business.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authService;
        public AccountController(IAuthenticationService authService)
        {
            this._authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestModel user)
        {
            try
            {
                return Ok(await _authService.Login(user));
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
                return Ok(await _authService.Registration(user));
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