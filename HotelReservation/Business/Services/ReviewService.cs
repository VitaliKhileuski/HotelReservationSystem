using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Helpers;
using Business.Interfaces;
using Business.Mappers;
using Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto.Prng;

namespace Business.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IMapper _reviewCategoryMapper;
        private readonly IMapper _reviewMapper;
        private readonly IReviewCategoryRepository _reviewCategoryRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly ILogger<ReviewService> _logger;
        public ReviewService(ILogger<ReviewService> logger, MapConfiguration cfg, IReviewCategoryRepository reviewCategoryRepository,
            IUserRepository userERepository, IOrderRepository orderRepository, IReviewRepository reviewRepository, IHotelRepository hotelRepository)
        {
            _reviewCategoryMapper = new Mapper(cfg.ReviewCategoryConfiguration);
            _reviewMapper = new Mapper(cfg.ReviewConfiguration);
            _logger = logger;
            _reviewCategoryRepository = reviewCategoryRepository;
            _userRepository = userERepository;
            _orderRepository = orderRepository;
            _reviewRepository = reviewRepository;
            _hotelRepository = hotelRepository;
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

        public async Task CreateReview(Guid orderId, string userId, ReviewModel review)
        {
            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }

            var orderEntity = await _orderRepository.GetAsync(orderId);
            if (orderEntity == null)
            {
                _logger.LogError($"order with {orderId} id not exists");
                throw new NotFoundException($"order with {orderId} id not exists");
            }

            var reviewCategoryWithRatingsEntities = new List<ReviewCategoryWithRatingEntity>();
            foreach (var reviewCategoryWithRating in review.Ratings)
            {
                reviewCategoryWithRatingsEntities.Add(new ReviewCategoryWithRatingEntity()
                {
                    Category = await _reviewCategoryRepository.GetAsync(reviewCategoryWithRating.Category.Id),
                    Rating = reviewCategoryWithRating.Rating
                });
            }

            var hotelEntity = orderEntity.Room.Hotel;
            hotelEntity.AverageRating = RecalculateHotelAverageRating(hotelEntity, reviewCategoryWithRatingsEntities);
            RecalculateAverageReviewCategoriesRatings(hotelEntity,reviewCategoryWithRatingsEntities);
            var reviewEntity = _reviewMapper.Map<ReviewModel, ReviewEntity>(review);
            if (reviewCategoryWithRatingsEntities.Count != 0)
            {
                reviewEntity.AverageRating = Math.Round(reviewCategoryWithRatingsEntities.Select(x => x.Rating).Sum() /
                                             reviewCategoryWithRatingsEntities.Count,2);
            }
            reviewEntity.Ratings = reviewCategoryWithRatingsEntities;
            reviewEntity.CreatedAt = DateTime.Now;
            reviewEntity.User = userEntity;
            reviewEntity.Order = orderEntity;
            reviewEntity.Hotel = hotelEntity;
            hotelEntity.Reviews.Add(reviewEntity);
            await _reviewRepository.CreateAsync(reviewEntity);
            await _hotelRepository.UpdateAsync(hotelEntity);

        }

        public async Task<PageInfo<ReviewModel>> GetFilteredReviews(Guid hotelId, Pagination reviewPagination)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }

            var reviewsModels = _reviewMapper.Map<ICollection<ReviewModel>>(hotelEntity.Reviews);

            return PageInfoCreator<ReviewModel>.GetPageInfo(reviewsModels, reviewPagination);
        }

        public IEnumerable<ReviewCategoryModel> GetAllReviewCategories()
        {
            return _reviewCategoryMapper.Map<IEnumerable<ReviewCategoryModel>>(_reviewCategoryRepository.GetAll());
        }

        private static double RecalculateHotelAverageRating(HotelEntity hotel, ICollection<ReviewCategoryWithRatingEntity> categoriesWithRatings)
        {
            var allHotelRatingsCount = hotel.Reviews.SelectMany(x => x.Ratings).Count();
            var ratingsSum = categoriesWithRatings.Select(x => x.Rating).Sum();
            double hotelAverageRating = ratingsSum / categoriesWithRatings.Count;
            if (hotel.AverageRating != null)
            {
                hotelAverageRating = (double) ((hotel.AverageRating * allHotelRatingsCount + ratingsSum) /
                                               (allHotelRatingsCount + categoriesWithRatings.Count));
            }
            return Math.Round(hotelAverageRating,2);

        }
        private static void RecalculateAverageReviewCategoriesRatings (HotelEntity hotel, ICollection<ReviewCategoryWithRatingEntity> categoriesWithRatings)
        {
            foreach(var categoryWithRating in categoriesWithRatings)
            {
                var categoryWithAverageRating =
                    hotel.AverageCategoryRatings.FirstOrDefault(x => x.CategoryId == categoryWithRating.Category.Id);
                if ( categoryWithAverageRating == null)
                {
                    hotel.AverageCategoryRatings.Add(new AverageReviewCategoryRatingsEntity
                    {
                        AverageRating = categoryWithRating.Rating,
                        Category = categoryWithRating.Category,
                        NumberOfReviews = 1
                    });
                }
                else
                {
                    var newAverageRating =(categoryWithAverageRating.AverageRating * categoryWithAverageRating.NumberOfReviews +
                                            categoryWithRating.Rating) / (categoryWithAverageRating.NumberOfReviews + 1);
                    if (newAverageRating != null)
                    {
                        categoryWithAverageRating.AverageRating = Math.Round((double)newAverageRating, 2);
                    }

                    categoryWithAverageRating.NumberOfReviews+=1;
                }
            }
        }

    }
}
