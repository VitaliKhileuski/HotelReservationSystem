using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailVerificationController : Controller
    {
        private readonly IEmailSenderService _emailSenderService;

        public EmailVerificationController(IEmailSenderService emailSenderService)
        {
            
            _emailSenderService = emailSenderService;
        }

        [HttpPost]
        [Authorize]
        [Route("{userId}")]
        public async Task<IActionResult> CreateEmailVerificationCode(Guid userId)
        {
           await _emailSenderService.CreateEmailVerificationCode(userId);
           return Ok();
        }

        [HttpPut]
        [Authorize]
        [Route("{userId}")]
        public async Task<IActionResult> CheckVerificationCode(Guid userId, [FromQuery] string verificationCode)
        {
            var result = await _emailSenderService.CheckVerificationCode(userId, verificationCode);
            return Ok(result);
        }
    }
}
