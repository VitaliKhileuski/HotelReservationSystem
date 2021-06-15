using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using HotelReservation.Api.Models.RequestModels;
using HotelReservation.Api.Models.ResponseModels;
using HotelReservation.Api.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        [Route("{hotelId:int}/{userId:int}/setHotelImage")]
        public async Task<IActionResult> EditHotelImage(int hotelId, int userId, [FromBody] ImageRequestModel image)
        {
            var idClaim = GetIdFromClaims();
            await _imageService.AddImageToHotel(image.Image, hotelId, userId);
            return Ok("Updated Successfully");
        }

        [HttpGet]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        [Route("{hotelId:int}/{userId:int}/getHotelImage")]
        public async Task<IActionResult> GetHotelImage(int hotelId, int userId)
        {
            var idClaim = GetIdFromClaims();
           var imageData =  await _imageService.GetHotelImage(hotelId, idClaim);
            var image = new ImageResponseModel
            {
                Image = imageData
            };
            return Ok(image);
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
