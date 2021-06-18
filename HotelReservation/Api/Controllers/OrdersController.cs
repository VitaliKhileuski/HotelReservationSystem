using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using HotelReservation.Api.Mappers;
using HotelReservation.Api.Models.RequestModels;
using HotelReservation.Api.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using HotelReservation.Api.Helpers;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly Mapper _mapper;

        public OrdersController(IOrderService orderService,CustomMapperConfiguration cfg)
        {
            _orderService = orderService;
            _mapper = new Mapper(cfg.OrderConfiguration);
        }

        [HttpGet]
        [Authorize]
        [Route("{orderId:int}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);
            var result = _mapper.Map<OrderModel, OrderResponseModel>(order);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [Route("{roomId:int}/order")]
        public async Task<IActionResult> CreateOrder(int roomId, [FromBody] OrderRequestModel order)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            var orderModel = _mapper.Map<OrderRequestModel, OrderModel>(order);
            orderModel.Services = new List<ServiceModel>();
            foreach(var id in order.ServicesId)
            {
                orderModel.Services.Add(new ServiceModel { Id = id });
            }
            await _orderService.CreateOrder(roomId, userId, orderModel);
            return Ok("Ordered");
        }

        [HttpPut]
        [Authorize]
        [Route("{orderId:int}/updateOrder")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] OrderRequestModel order)
        {
            var orderModel = _mapper.Map<OrderRequestModel, OrderModel>(order);
            orderModel.Services = new List<ServiceModel>();
            foreach (var id in order.ServicesId)
            {
                orderModel.Services.Add(new ServiceModel { Id = id });
            }

            await _orderService.UpdateOrder(orderId, orderModel);
            return Ok("Updated successfully");
        }

        [HttpDelete]
        [Authorize]
        [Route("{orderId:int}/deleteOrder")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            await _orderService.DeleteOrder(orderId);
            return Ok("Deleted Successfully");
        }
    }
}