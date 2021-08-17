using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Mappers;
using Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IMapper _reviewCategoryMapper;
        private readonly IReviewCategoryRepository _reviewCategoryRepository;
        private readonly ILogger<ReviewService> _logger;
        public ReviewService(ILogger<ReviewService> logger, MapConfiguration cfg, IReviewCategoryRepository reviewCategoryRepository)
        {
            _reviewCategoryMapper = new Mapper(cfg.ReviewCategoryConfiguration);
            _logger = logger;
            _reviewCategoryRepository = reviewCategoryRepository;
        }
        public async Task CreateReviewCategory(ReviewCategoryModel reviewCategory)
        {

            var reviewCategoryEntity = await  _reviewCategoryRepository.GetReviewCategoryByName(reviewCategory.Name);
            if (reviewCategoryEntity != null)
            {
                _logger.LogError("review category with that name already exists");
                throw new BadRequestException("review category with that name already exists");
            }

            reviewCategoryEntity = _reviewCategoryMapper.Map<ReviewCategoryModel, ReviewCategoryEntity>(reviewCategory);
            await _reviewCategoryRepository.CreateAsync(reviewCategoryEntity);
        }

        public async Task DeleteReviewCategory(Guid reviewCategoryId)
        {
            var reviewCategoryEntity = await _reviewCategoryRepository.GetAsync(reviewCategoryId);
            if (reviewCategoryEntity == null)
            {
                _logger.LogError($"review category with {reviewCategoryId} id not exists");
                throw new NotFoundException($"review category with {reviewCategoryId} id not exists");
            }

            await _reviewCategoryRepository.DeleteAsync(reviewCategoryId);

        }

        public IEnumerable<ReviewCategoryModel> GetAllReviewCategories()
        {
            return _reviewCategoryMapper.Map<IEnumerable<ReviewCategoryModel>>(_reviewCategoryRepository.GetAll());
        }
    }
}
