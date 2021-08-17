using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
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
        public ReviewController(CustomMapperConfiguration cfg, IReviewService reviewService)
        {
            _reviewCategoryMapper = new Mapper(cfg.ReviewCategoryConfiguration);
            _reviewService = reviewService;
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

        [HttpDelete]
        [Authorize]
        [Route("{reviewCategoryId}/deleteReviewCategory")]
        public async Task<IActionResult> DeleteReviewCategory(Guid reviewCategoryId)
        {
            await _reviewService.DeleteReviewCategory(reviewCategoryId);
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
