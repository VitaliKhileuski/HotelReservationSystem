using System;
using System.Collections.Generic;
using System.Text;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class FileContentRepository : BaseRepository<FileContentEntity>, IFileContentRepository
    {
        private readonly Context _db;

        public FileContentRepository(Context context) : base(context)
        {
            _db = context;
        }
    }
}
