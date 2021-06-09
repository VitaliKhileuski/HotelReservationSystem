using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Models;
using HotelReservation.Api.Mappers;
using HotelReservation.Api.Models.RequestModels;
using HotelReservation.Api.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using HotelReservation.Api.Policy;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : Controller
    {
        private readonly IFacilityService _facilitiesService;
        private readonly Mapper _mapper;

        public ServicesController(IFacilityService facilitiesService, CustomMapperConfiguration cfg)
        {
            _facilitiesService = facilitiesService;
            _mapper = new Mapper(cfg.ServiceConfiguration);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllServices()
        {
            var orders = _mapper.Map<ICollection<ServiceResponseModel>>(_facilitiesService.GetAllServices());
            return Ok(orders);
        }

        [HttpGet]
        [Route("{serviceId:int}")]
        [Authorize]
        public async Task<IActionResult> GetServiceById(int serviceId)
        {
            var service = _mapper.Map<ServiceModel, ServiceResponseModel>(await _facilitiesService.GetServiceById(serviceId));
            return Ok(service);
        }

        [HttpPost]
        [Route("{hotelId:int}/addService")]
        [Authorize(Policy = Policies.AdminPermission)]
        public async Task<IActionResult> AddServiceToHotel(int hotelId, [FromBody] ServiceRequestModel service)
        {
            var serviceModel = _mapper.Map<ServiceRequestModel, ServiceModel>(service);
            var userId = GetIdFromClaims();
            await _facilitiesService.AddServiceToHotel(hotelId, userId, serviceModel);
            return Ok("added successfully");
        }

        [HttpDelete]
        [Route("{serviceId:int}/deleteService")]
        [Authorize(Policy = "HotelAdminPermission")]
        public async Task<IActionResult> DeleteServiceFromHotel(int serviceId)
        {
            var userId = GetIdFromClaims();
            await _facilitiesService.DeleteOrderFromHotel(serviceId, userId);
            return Ok("deleted");
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