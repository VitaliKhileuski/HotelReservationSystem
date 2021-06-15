using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Mappers;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class ImageService : IImageService
    {
        private readonly IBaseRepository<ImageEntity> _imageRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IUserRepository _userRepository;
        private readonly Mapper _imageMapper;
        private readonly ILogger<ImageEntity> _logger;

        public ImageService(IBaseRepository<ImageEntity> imageRepository,IHotelRepository hotelRepository,IUserRepository userRepository,
            ILogger<ImageEntity> logger,  MapConfiguration cfg)
        {
            _imageMapper = new Mapper(cfg.ImageConfiguration);
            _imageRepository = imageRepository;
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task AddImageToHotel(string image,int hotelId,int userId)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }

            var userEntity = await _userRepository.GetAsync(userId);
             
            if (hotelEntity.Admins.FirstOrDefault(x => x.Id == userId) != null || userEntity.Role.Name == "Admin")
            {
                var imageEntity = new ImageEntity
                {
                    ImageData = ImageConverter(image)
                };
                hotelEntity.Image = imageEntity;
                await _hotelRepository.UpdateAsync(hotelEntity);
            }
            else
            {
                _logger.LogError("you don't have permission to edit this hotel");
                throw new BadRequestException("you don't have permission to edit this hotel");
            }
        }

        public async Task<string> GetHotelImage(int hotelId, int userId)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }

            var userEntity = await _userRepository.GetAsync(userId);

            if (hotelEntity.Admins.FirstOrDefault(x => x.Id == userId) != null || userEntity.Role.Name == "Admin")
            {
                if (hotelEntity.Image == null)
                {
                    return null;
                }
                var image = hotelEntity.Image.ImageData;
                return ImageConverter(image);
            }

            _logger.LogError("you don't have permission to get image from this hotel");
            throw new BadRequestException("you don't have permission to get image from this hotel");
        }

        private  byte[] ImageConverter(string image)
        {
            var imageData = Convert.FromBase64String(image);

            return imageData;
        }

        private string ImageConverter(byte[] image)
        {
            var base64Image = Convert.ToBase64String(image);
            return base64Image;
        }
    }
}
