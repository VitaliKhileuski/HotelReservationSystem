using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Models.FilterModels;
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
    public class ReviewController : Controller
    {
        private readonly IMapper _reviewCategoryMapper;
        private readonly IReviewService _reviewService;
        private readonly IMapper _reviewMapper;
        public ReviewController(CustomMapperConfiguration cfg, IReviewService reviewService)
        {
            _reviewCategoryMapper = new Mapper(cfg.ReviewCategoryConfiguration);
            _reviewService = reviewService;
            _reviewMapper = new Mapper(cfg.ReviewConfiguration);
        }

        [HttpGet]
        [Authorize]
        [Route("getAllReviewCategories")]
        public IActionResult  GetAllReviewCategories()
        {
            var result =
                _reviewCategoryMapper.Map<IEnumerable<ReviewCategoryResponseModel>>(_reviewService
                    .GetAllReviewCategories());
            return Ok(result);
        }
        [HttpGet]
        [Route("{hotelId}/page")]
        public async Task<IActionResult> GetFilteredReviews(Guid hotelId, [FromQuery] Pagination filter)
        {

            var validFilter = new Pagination(filter.PageNumber, filter.PageSize);
            var pageInfo = await _reviewService.GetFilteredReviews(hotelId, validFilter);
            var reviews  = _reviewMapper.Map<List<ReviewResponseModel>>(pageInfo.Items);
            var responsePageInfo = new PageInfo<ReviewResponseModel>
            {
                Items = reviews,
                NumberOfItems = pageInfo.NumberOfItems,
                NumberOfPages = pageInfo.NumberOfPages
            };

            return Ok(responsePageInfo);
        }

        [HttpDelete]
        [Authorize]
        [Route("{reviewCategoryId}/deleteReviewCategory")]
        public async Task<IActionResult> DeleteReviewCategory(Guid reviewCategoryId)
        {
            await _reviewService.DeleteReviewCategory(reviewCategoryId);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("{orderId}/createReview")]
        public async Task<IActionResult> CreateReview(Guid orderId,
            [FromBody] ReviewRequestModel review)
        {
            var userId = TokenData.GetIdFromClaims(User.Claims);
            var reviewModel = _reviewMapper.Map<ReviewRequestModel, ReviewModel>(review);
           await _reviewService.CreateReview(orderId, userId, reviewModel);
            return Ok();
        }

        [HttpPost]
        [Authorize(Policy = Policies.AdminPermission)]
        [Route("addCategory")]
        public async Task<IActionResult> CreateReviewCategory(ReviewCategoryRequestModel reviewCategory)
        {
            var reviewCategoryEntity =
                _reviewCategoryMapper.Map<ReviewCategoryRequestModel, ReviewCategoryModel>(reviewCategory);
            await _reviewService.CreateReviewCategory(reviewCategoryEntity);
            return Ok();
        }
        
    }
}
