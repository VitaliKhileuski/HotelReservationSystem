using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly Mapper _imageMapper;
        private readonly ILogger<ImageEntity> _logger;

        public ImageService(IImageRepository imageRepository,IHotelRepository hotelRepository,IUserRepository userRepository,
            IRoomRepository roomRepository, ILogger<ImageEntity> logger,  MapConfiguration cfg)
        {
            _imageMapper = new Mapper(cfg.ImageConfiguration);
            _imageRepository = imageRepository;
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _logger = logger;
        }

        public async Task AddImageToHotel(ImageModel image,int hotelId,int userId)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }

            var userEntity = await _userRepository.GetAsync(userId);
             
            if (PermissionVerifier.CheckPermission(hotelEntity,userEntity))
            {
              var imageEntity = _imageMapper.Map<ImageModel, ImageEntity>(image);
                if (hotelEntity.Image != null)
                {
                    await _imageRepository.DeleteAsync(hotelEntity.Image.Id);
                }
                hotelEntity.Image = imageEntity;
                await _hotelRepository.UpdateAsync(hotelEntity);
            }
        }

        public async Task<ImageModel> GetHotelImage(int hotelId)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }
            
            if (hotelEntity.Image == null)
            {
                return null;
            }

            var imageModel = _imageMapper.Map<ImageEntity, ImageModel>(hotelEntity.Image); 
            return imageModel;
        }

        public async Task<List<ImageModel>> GetRoomImages(int roomId)
        {
            var roomEntity = await _roomRepository.GetAsync(roomId);
            if (roomEntity == null)
            {
                _logger.LogError($"room with {roomId} id not exists");
                throw new NotFoundException($"room with {roomId} id not exists");
            }

            if (roomEntity.Images == null)
            {
                return new List<ImageModel>();
            }

            var roomImages = _imageMapper.Map<List<ImageModel>>(roomEntity.Images);
               
            return roomImages;
            
        }

        public async Task SetImagesToRoom(List<ImageModel> imagesData, int roomId, int userId)
        {
            var roomEntity = await _roomRepository.GetAsync(roomId);
            if (roomEntity == null)
            {
                _logger.LogError($"room with {roomId} id not exists");
                throw new NotFoundException($"room with {roomId} id not exists");
            }
            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }

            if (PermissionVerifier.CheckPermission(roomEntity.Hotel, userEntity))
            {
                if (roomEntity.Images == null)
                {
                    roomEntity.Images = new List<ImageEntity>();
                }
                else
                {
                    var images = _imageMapper.Map<List<ImageEntity>>(imagesData);

                    var imageIds = roomEntity.Images.Select(image => image.Id).ToList();

                    foreach (var id in imageIds)
                    {
                        await _imageRepository.DeleteAsync(id);
                    }
                    roomEntity.Images = images;
                }

                await _roomRepository.UpdateAsync(roomEntity); 
            }
        }
    }
}
