using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Models;
using Business.Services;
using HotelReservation.Api.Mappers;
using HotelReservation.Api.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : Controller
    {
        private readonly FacilitiesService _facilitiesService;
        private readonly Mapper _mapper;

        public ServicesController(FacilitiesService facilitiesService, CustomMapperConfiguration cfg)
        {
            _facilitiesService = facilitiesService;
            _mapper = new Mapper(cfg.ServiceConfiguration);
        }




        [HttpPost]
        [Route("{hotelId:int}/AddService")]
        [Authorize(Policy = "HotelAdminPermission")]
        public async Task<IActionResult> AddServiceToHotel(int hotelId, [FromBody] ServiceRequestModel service)
        {
            try
            {
                var serviceModel = _mapper.Map<ServiceRequestModel, ServiceModel>(service);
                var userId = GetIdFromClaims();
                await _facilitiesService.AddServiceToHotel(hotelId, userId, serviceModel);
                return Ok("added successfully");
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
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