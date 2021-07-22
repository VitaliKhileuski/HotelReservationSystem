using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAllServices()
        {
            var orders = _mapper.Map<ICollection<ServiceResponseModel>>(_facilitiesService.GetAllServices());
            return Ok(orders);
        }
        [HttpGet]
        [Route("{hotelId}/pages")]
        public async Task<IActionResult> GetPage(Guid hotelId, [FromQuery] Pagination filter,[FromQuery] SortModel sortModel)
        {
            var validFilter = new Pagination(filter.PageNumber, filter.PageSize);
            var pageInfo = await _facilitiesService.GetServicesPage(hotelId, validFilter, sortModel);
            var services = _mapper.Map<List<ServiceResponseModel>>(pageInfo.Items);
            var responsePageInfo = new PageInfo<ServiceResponseModel>
            {
                Items = services,
                NumberOfItems = pageInfo.NumberOfItems,
                NumberOfPages = pageInfo.NumberOfPages
            };

            return Ok(responsePageInfo);
        }

        [HttpGet]
        [Route("{serviceId}")]
        [Authorize]
        public async Task<IActionResult> GetServiceById(Guid serviceId)
        {
            var service = _mapper.Map<ServiceModel, ServiceResponseModel>(await _facilitiesService.GetServiceById(serviceId));
            return Ok(service);
        }
        [HttpPut]
        [Route("{serviceId}")]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        public async Task<IActionResult> UpdateService(Guid serviceId, [FromBody] ServiceRequestModel service)
        {
            var serviceModel = _mapper.Map<ServiceRequestModel, ServiceModel>(service);
            var userId = TokenData.GetIdFromClaims(User.Claims);
            await _facilitiesService.UpdateService(serviceId, userId, serviceModel);
            return Ok("Updated Successfully");
        }

        [HttpPost]
        [Route("{hotelId}")]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        public async Task<IActionResult> AddServiceToHotel(Guid hotelId, [FromBody] ServiceRequestModel service)
        {
            var serviceModel = _mapper.Map<ServiceRequestModel, ServiceModel>(service);
            var userId = TokenData.GetIdFromClaims(User.Claims);
            await _facilitiesService.AddServiceToHotel(hotelId, userId, serviceModel);
            return Ok("added successfully");
        }

        [HttpDelete]
        [Route("{serviceId}")]
        [Authorize(Policy = Policies.AllAdminsPermission)]
        public async Task<IActionResult> DeleteServiceFromHotel(Guid serviceId)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            await _facilitiesService.DeleteOrderFromHotel(serviceId, userId);
            return Ok();
        }
    }
}