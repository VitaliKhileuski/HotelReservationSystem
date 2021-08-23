using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Mappers;
using Business.Models;
using Business.Models.FilterModels;
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
        private readonly IMapper _updateOrderMapper;
        private readonly IMapper _reviewMapper;

        public OrdersController(IOrderService orderService,CustomMapperConfiguration cfg,MapConfiguration modelCfg)
        {
            _orderService = orderService;
            _mapper = new Mapper(cfg.OrderConfiguration);
            _updateOrderMapper = new Mapper(cfg.UpdateOrderConfiguration);
            _reviewMapper = new Mapper(cfg.ReviewConfiguration);

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
        public async Task<IActionResult> GetPage([FromQuery] OrderFilter orderFilter, [FromQuery] Pagination filter,[FromQuery] SortModel sortModel)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            var validFilter = new Pagination(filter.PageNumber, filter.PageSize);
            var pageInfo = await _orderService.GetOrdersPage(userId,orderFilter, validFilter,sortModel);
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

            var orderNumber =  await _orderService.CreateOrder(roomId, orderModel);
            return Ok(orderNumber);
        }

        [HttpPut]
        [Authorize]
        [Route("{orderId}/updateOrder")]
        public async Task<IActionResult> UpdateOrder(Guid orderId, [FromBody] UpdateOrderRequestModel updateOrderRequestModel)
        {
            var updateOrderModel = _updateOrderMapper.Map<UpdateOrderRequestModel, UpdateOrderModel>(updateOrderRequestModel);
            updateOrderModel.Services = new List<ServiceQuantityModel>();
            foreach (var serviceQuantity in updateOrderRequestModel.Services)
            {
                updateOrderModel.Services.Add(new ServiceQuantityModel
                {
                    Quantity = serviceQuantity.Quantity,
                    Service = new ServiceModel
                    {
                        Id = serviceQuantity.ServiceId
                    }
                });
            }

            await _orderService.UpdateOrder(orderId, updateOrderModel);
            return Ok("Updated successfully");
        }

        [HttpDelete]
        [Authorize]
        [Route("{orderId}/deleteOrder")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            await _orderService.DeleteOrder(orderId);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("{orderId}/getReview")]
        public async Task<IActionResult> GetOrderReview(Guid orderId)
        {
           var reviewModel = await _orderService.GetReview(orderId);
           var reviewResponseModel = _reviewMapper.Map<ReviewModel, ReviewResponseModel>(reviewModel);
           return Ok(reviewResponseModel);

        }
    }
}