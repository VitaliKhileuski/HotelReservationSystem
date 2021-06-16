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
using Business.Models;
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
        private readonly IRoomRepository _roomRepository;
        private readonly Mapper _imageMapper;
        private readonly ILogger<ImageEntity> _logger;

        public ImageService(IBaseRepository<ImageEntity> imageRepository,IHotelRepository hotelRepository,IUserRepository userRepository,
            IRoomRepository roomRepository, ILogger<ImageEntity> logger,  MapConfiguration cfg)
        {
            _imageMapper = new Mapper(cfg.ImageConfiguration);
            _imageRepository = imageRepository;
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
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
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }

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

        public async Task<List<string>> GetRoomImages(int roomId,int userId)
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

            if (roomEntity.Hotel.Admins.FirstOrDefault(x => x.Id == userId) != null || userEntity.Role.Name == "Admin")
            {
                if (roomEntity.Images == null)
                {
                    return null;
                }
                List<string> imagesbase64 = new List<string>();

                var image1 = await _imageRepository.GetAsync(1);
                var image2 = await _imageRepository.GetAsync(2);
                imagesbase64.Add(ImageConverter(image1.ImageData));
                imagesbase64.Add(ImageConverter(image2.ImageData));


                //foreach(var image in roomEntity.Images)
                //{
                //    imagesbase64.Add(ImageConverter(image.ImageData));
                //}
                return imagesbase64;
            }


            _logger.LogError("you don't have permission to get image from this hotel");
            throw new BadRequestException("you don't have permission to get image from this hotel");

        }

        public async Task SetImagesToRoom(List<string> imagesData, int roomId, int userId)
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

            if (roomEntity.Hotel.Admins.FirstOrDefault(x => x.Id == userId) != null || userEntity.Role.Name == "Admin")
            {
                if (roomEntity.Images == null)
                {
                    roomEntity.Images = new List<ImageEntity>();
                }
                else
                {
                    foreach (var image in imagesData)
                    {
                        roomEntity.Images.Add(new ImageEntity()
                        {
                            ImageData = ImageConverter(image)
                        });
                    }
                }

                await _roomRepository.UpdateAsync(roomEntity); 
            }
            else
            {
                _logger.LogError("you don't have permission to set images to this room");
                throw new BadRequestException("you don't have permission to set images to this room");
            }
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
