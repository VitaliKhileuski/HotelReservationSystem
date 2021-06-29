using System;
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
        private readonly IAttachmentsService _attachmentsService;
        private readonly Mapper _attachmentMapper;
        public ImagesController(IAttachmentsService attachmentsService, CustomMapperConfiguration cfg)
        {
            _attachmentsService = attachmentsService;
            _attachmentMapper = new Mapper(cfg.AttachmentConfiguration);  //attach
        }

        [HttpPost]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        [Route("{hotelId}/setHotelImages")]
        public async Task<IActionResult> EditHotelImage(Guid hotelId,[FromBody] List<FileRequestModel> files)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            var attachmentModels = _attachmentMapper.Map<List<AttachmentModel>>(files);
            await _attachmentsService.SetImagesToHotel(attachmentModels, hotelId, userId);
            return Ok();
        }

        [HttpPost]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        [Route("{roomId}/setRoomImages")]
        public async Task<IActionResult> SetRoomImages(Guid roomId, [FromBody] List<FileRequestModel> files)
        {
            var attachmentModels = _attachmentMapper.Map<List<AttachmentModel>>(files);
            var userId = TokenData.GetIdFromClaims(User.Claims);
            await _attachmentsService.SetImagesToRoom(attachmentModels, roomId, userId);
            return Ok();
        }

        [HttpGet]
        [Route("{fileId:guid}")]
        public async Task<FileContentResult> GetHotelImage(Guid fileId)
        {
            var imageData =  await _attachmentsService.GetImage(fileId);
            var file = new FileContentResult(imageData.FileContent.Content, imageData.FileExtension)
            {
                FileDownloadName = imageData.FileName
            };
            
            return file;
        }
        [HttpGet]
        [Route("{fileId:guid}/imageInfo")]
        public async Task<IActionResult> GetHotelImageInfo(Guid fileId)
        {
            var imageData = await _attachmentsService.GetImage(fileId);
            var file = new FileContentResult(imageData.FileContent.Content, imageData.FileExtension)
            {
                FileDownloadName = imageData.FileName
            };

            return Ok(file);
        }
    }
}