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

        [HttpPost]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        [Route("{roomId:int}/setRoomImages")]
        public async Task<IActionResult> SetRoomImages(int roomId, [FromBody] List<ImageRequestModel> images)
        {
            List<string> imagesData = new List<string>();
            foreach(var image in images)
            {
                imagesData.Add(image.Image);
            }
            var idClaim = GetIdFromClaims();
            await _imageService.SetImagesToRoom(imagesData, roomId, idClaim);
            return Ok("Updated Successfully");
        }

        [HttpGet]
        [Route("{hotelId:int}/{userId:int}/getHotelImage")]
        public async Task<IActionResult> GetHotelImage(int hotelId, int userId)
        {
            var imageData =  await _imageService.GetHotelImage(hotelId);
            var image = new ImageResponseModel
            {
                Image = imageData
            };
            return Ok(image);
        }
        [HttpGet]
        [Route("{roomId:int}/getRoomImages")]
        public async Task<IActionResult> GetRoomImages(int roomId)
        {
            var imagesData = await _imageService.GetRoomImages(roomId);
            List<ImageResponseModel> imageResponseModels = new List<ImageResponseModel>();
            foreach(var image in imagesData)
            {
                imageResponseModels.Add(new ImageResponseModel()
                {
                    Image = image
                });
            }
            return Ok(imageResponseModels);
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
