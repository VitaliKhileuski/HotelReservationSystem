using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Mappers;
using Business.Models;
using HotelReservation.Api.Mappers;
using HotelReservation.Api.Models.RequestModels;
using HotelReservation.Api.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using HotelReservation.Api.Helpers;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly Mapper _mapper;
        private readonly IMapper _modelMapper;


        public OrdersController(IOrderService orderService,CustomMapperConfiguration cfg,MapConfiguration modelCfg)
        {
            _orderService = orderService;
            _mapper = new Mapper(cfg.OrderConfiguration);
            _modelMapper = new Mapper(modelCfg.ServiceConfiguration);
        }

        [HttpGet]
        [Authorize]
        [Route("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var order = await _orderService.GetOrderById(orderId);
            var result = _mapper.Map<OrderModel, OrderResponseModel>(order);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPage([FromQuery] Pagination filter)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            var validFilter = new Pagination(filter.PageNumber, filter.PageSize);
            var pageInfo = await _orderService.GetOrdersPage(userId, validFilter);
            var orders = _mapper.Map<List<OrderResponseModel>>(pageInfo.Items);
            var responsePageInfo = new PageInfo<OrderResponseModel>
            {
                Items = orders,
                NumberOfItems = pageInfo.NumberOfItems,
                NumberOfPages = pageInfo.NumberOfPages
            };

            return Ok(responsePageInfo);
        }

        [HttpPost]
        [Route("{roomId}/order")]
        public async Task<IActionResult> CreateOrder(Guid roomId, [FromBody] OrderRequestModel order)
        {
            var orderModel = _mapper.Map<OrderRequestModel, OrderModel>(order);
            orderModel.Services = new List<ServiceQuantityModel>();
            foreach (var serviceQuantity in order.ServiceQuantities)
            {
                orderModel.Services.Add(new ServiceQuantityModel
                {
                    Quantity = serviceQuantity.Quantity,
                    Service = new ServiceModel
                    {
                        Id =  serviceQuantity.ServiceId
                    }
                });
            }

            await _orderService.CreateOrder(roomId, orderModel);
            return Ok("Ordered");
        }

        //[HttpPut]
        //[Authorize]
        //[Route("{orderId}/updateOrder")]
        //public async Task<IActionResult> UpdateOrder(Guid orderId, [FromBody] OrderRequestModel order)
        //{
        //    var orderModel = _mapper.Map<OrderRequestModel, OrderModel>(order);
        //    orderModel.Services = new List<ServiceModel>();

        //    await _orderService.UpdateOrder(orderId, orderModel);
        //    return Ok("Updated successfully");
        //}

        [HttpDelete]
        [Authorize]
        [Route("{orderId}/deleteOrder")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            await _orderService.DeleteOrder(orderId);
            return Ok();
        }
    }
}