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
using Org.BouncyCastle.Bcpg;

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

        //[HttpGet]
        //[Authorize]
        //public async Task<IActionResult> GetAllOrders()
        //{
        //    try
        //    {
        //        var userId = GetIdFromClaims();

        //    }
        //    catch (NotFoundException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost]
        [Authorize]
        [Route("{roomId:int}/order")]
        public async Task<IActionResult> CreateOrder(int roomId,[FromBody] OrderRequestModel order)
        {
            try
            {
                var userId = GetIdFromClaims();
                var orderModel = _mapper.Map<OrderRequestModel, OrderModel>(order);
              await _orderService.CreateOrder(roomId,userId,orderModel);
              return Ok("Ordered");
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
