using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IAttachmentsService
    {
        Task SetImagesToHotel(List<AttachmentModel> attachments, Guid hotelId,string userId);
        Task<AttachmentModel> GetImage(Guid attachmentId);
        Task SetImagesToRoom(List<AttachmentModel> attachments,Guid roomId,string userId);
    }
}