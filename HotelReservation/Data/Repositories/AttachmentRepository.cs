using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class AttachmentRepository : BaseRepository<AttachmentEntity> , IAttachmentRepository
    {
        private readonly Context _db;

        public AttachmentRepository(Context context) : base(context)
        {
            _db = context;
        }
    }
}