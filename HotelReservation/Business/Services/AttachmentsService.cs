using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AttachmentsService : IAttachmentsService
    {
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IFileContentRepository _fileContentRepository;
        private readonly Mapper _attachmentMapper;
        private readonly ILogger<AttachmentEntity> _logger;

        public AttachmentsService(IAttachmentRepository attachmentRepository,IHotelRepository hotelRepository,IUserRepository userRepository,
            IRoomRepository roomRepository,IFileContentRepository fileContentRepository, ILogger<AttachmentEntity> logger,  MapConfiguration cfg)
        {
            _attachmentMapper = new Mapper(cfg.AttachmentConfiguration);
            _attachmentRepository = attachmentRepository;
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _fileContentRepository = fileContentRepository;
            _logger = logger;
        }

        public async Task SetImagesToHotel(List<AttachmentModel> attachments, Guid hotelId, string userId)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }

            var userEntity = await _userRepository.GetAsync(userId);

            if (PermissionVerifier.CheckPermission(hotelEntity, userEntity))
            {
                if (hotelEntity.Attachments == null)
                {
                    hotelEntity.Attachments = new List<AttachmentEntity>();
                }
                else
                {
                    var attachmentEntities = _attachmentMapper.Map<List<AttachmentEntity>>(attachments);

                    var attachmentsIds = hotelEntity.Attachments.Select(image => image.FileContent.Id).ToList();

                    foreach (var id in attachmentsIds)
                    {
                        await _fileContentRepository.DeleteAsync(id);
                    }
                    hotelEntity.Attachments = attachmentEntities;
                }
                await _hotelRepository.UpdateAsync(hotelEntity);
            }
        }

        public async Task<AttachmentModel> GetImage(Guid attachmentId)
        {
            var attachmentEntity = await _attachmentRepository.GetAsync(attachmentId);
            if (attachmentEntity == null)
            {
                _logger.LogError($"attachment with {attachmentId} id not exists");
                throw new NotFoundException($"attachment with {attachmentId} id not exists");
            }
            var imageModel = _attachmentMapper.Map<AttachmentEntity, AttachmentModel>(attachmentEntity); 
            return imageModel;
        }


        public async Task SetImagesToRoom(List<AttachmentModel> attachments, Guid roomId, string userId)
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
                if (roomEntity.Attachments == null)
                {
                    roomEntity.Attachments = new List<AttachmentEntity>();
                }
                else
                {
                    var attachmentEntities = _attachmentMapper.Map<List<AttachmentEntity>>(attachments);

                    var imageIds = roomEntity.Attachments.Select(image => image.FileContent.Id).ToList();

                    foreach (var id in imageIds)
                    {
                        await _fileContentRepository.DeleteAsync(id);
                    }
                    roomEntity.Attachments = attachmentEntities;
                }
                await _roomRepository.UpdateAsync(roomEntity); 
            }
        }
    }
}
