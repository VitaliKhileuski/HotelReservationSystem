using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class ServiceRepository : BaseRepository<ServiceEntity> , IServiceRepository
    {
        private readonly Context _db;

        public ServiceRepository(Context context) : base(context)
        {
            _db = context;
        }

        public IEnumerable<ServiceEntity> GetServicesPageFromHotel(int pageNumber, int pageSize, int hotelId)
        {
            return _db.Services.Skip((pageNumber - 1) * pageSize).Take(pageSize).Where(x => x.HotelId == hotelId);
        }

        public async Task<int> GetServiceCount(int hotelId)
        {
            return await _db.Services.Where(x => x.HotelId == hotelId).CountAsync();
        }
    }
}