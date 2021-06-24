using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using HotelReservation.Api.Helpers;
using HotelReservation.Api.Mappers;
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
        private readonly Mapper _imageMapper;

        public ImagesController(IImageService imageService, CustomMapperConfiguration cfg)
        {
            _imageService = imageService;
            _imageMapper = new Mapper(cfg.ImageConfiguration);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        [Route("{hotelId:int}/setHotelImage")]
        public async Task<IActionResult> EditHotelImage(int hotelId,[FromBody] ImageRequestModel image)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            var imageModel = _imageMapper.Map<ImageRequestModel, ImageModel>(image);
            await _imageService.AddImageToHotel(imageModel, hotelId, userId);
            return Ok();
        }

        [HttpPost]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        [Route("{roomId:int}/setRoomImages")]
        public async Task<IActionResult> SetRoomImages(int roomId, [FromBody] List<ImageRequestModel> images)
        {
            var imageModels = _imageMapper.Map<List<ImageModel>>(images);
            var userId = TokenData.GetIdFromClaims(User.Claims);
            await _imageService.SetImagesToRoom(imageModels, roomId, userId);
            return Ok();
        }

        [HttpGet]
        [Route("{hotelId:int}/getHotelImage")]
        public async Task<IActionResult> GetHotelImage(int hotelId)
        {
            var imageData =  await _imageService.GetHotelImage(hotelId);
            var imageResponse = _imageMapper.Map<ImageModel, ImageResponseModel>(imageData);
            return Ok(imageResponse);
        }

        [HttpGet]
        [Route("{roomId:int}/getRoomImages")]
        public async Task<IActionResult> GetRoomImages(int roomId)
        {
            var imagesData = await _imageService.GetRoomImages(roomId);

            var responseImages = _imageMapper.Map<ICollection<ImageResponseModel>>(imagesData);
            return Ok(responseImages);
        }
    }
}