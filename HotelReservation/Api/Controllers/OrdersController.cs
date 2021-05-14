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
using HotelReservation.Api.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly OrdersService _orderService;
        private readonly Mapper _mapper;

        public OrdersController(OrdersService orderService,CustomMapperConfiguration cfg)
        {
            _orderService = orderService;
            _mapper = new Mapper(cfg.OrderConfiguration);
        }



        [HttpGet]
        [Authorize]
        [Route("{orderId:int}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
              var order = await _orderService.GetOrderById(orderId);
              var result = _mapper.Map<OrderModel, OrderResponseModel>(order);
              return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            try
            {
                var orders = _orderService.GetAll();
                var result = _mapper.Map<List<OrderResponseModel>>(orders);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("{roomId:int}/order")]
        public async Task<IActionResult> CreateOrder(int roomId, [FromBody] OrderRequestModel order)
        {
            try
            {
                var userId = GetIdFromClaims();
                var orderModel = _mapper.Map<OrderRequestModel, OrderModel>(order);
                await _orderService.CreateOrder(roomId, userId, orderModel);
                return Ok("Ordered");
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        [Route("{roomId:int}/updateOrder")]
        public async Task<IActionResult> UpdateOrder(int roomId, [FromBody] OrderRequestModel order)
        {
            try
            {
                var orderModel = _mapper.Map<OrderRequestModel, OrderModel>(order);
                await _orderService.UpdateOrder(roomId, orderModel);
                return Ok("Updated successfully");
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("{orderId:int}/deleteOrder")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            try
            {
                await _orderService.DeleteOrder(orderId);
                return Ok("Deleted Successfully");
            }
            catch (NotFoundException ex)
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
