using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Models;
using HotelReservation.Api.Helpers;
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
        [Route("{hotelId:int}/pages")]
        public async Task<IActionResult> GetPage(int hotelId, [FromQuery] Pagination filter)
        {
            var validFilter = new Pagination(filter.PageNumber, filter.PageSize);
            var servicesWithCount = await _facilitiesService.GetServicesPage(hotelId, validFilter);
            var services = _mapper.Map<List<ServiceResponseModel>>(servicesWithCount.Item1);
            var maxNumberOfServices = servicesWithCount.Item2;

            return Ok(Tuple.Create(services, maxNumberOfServices));
        }

        [HttpGet]
        [Route("{serviceId:int}")]
        [Authorize]
        public async Task<IActionResult> GetServiceById(int serviceId)
        {
            var service = _mapper.Map<ServiceModel, ServiceResponseModel>(await _facilitiesService.GetServiceById(serviceId));
            return Ok(service);
        }
        [HttpPut]
        [Route("{serviceId:int}")]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        public async Task<IActionResult> UpdateService(int serviceId, [FromBody] ServiceRequestModel service)
        {
            var serviceModel = _mapper.Map<ServiceRequestModel, ServiceModel>(service);
            var userId = TokenData.GetIdFromClaims(User.Claims);
            await _facilitiesService.UpdateService(serviceId, userId, serviceModel);
            return Ok("Updated Successfully");
        }

        [HttpPost]
        [Route("{hotelId:int}")]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        public async Task<IActionResult> AddServiceToHotel(int hotelId, [FromBody] ServiceRequestModel service)
        {
            var serviceModel = _mapper.Map<ServiceRequestModel, ServiceModel>(service);
            var userId = TokenData.GetIdFromClaims(User.Claims);
            await _facilitiesService.AddServiceToHotel(hotelId, userId, serviceModel);
            return Ok("added successfully");
        }

        [HttpDelete]
        [Route("{serviceId:int}")]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        public async Task<IActionResult> DeleteServiceFromHotel(int serviceId)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            await _facilitiesService.DeleteOrderFromHotel(serviceId, userId);
            return Ok();
        }
    }
}