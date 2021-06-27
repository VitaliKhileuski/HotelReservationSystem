using System;

namespace HotelReservation.Data.Entities
{
    public class FileContentEntity : Entity
    {
        public byte[] Content { get; set; }
        public Guid AttachmentId { get; set; }
        public virtual AttachmentEntity Attachment { get; set; }
    }
}