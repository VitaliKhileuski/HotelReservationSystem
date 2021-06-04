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
    public class ServiceRepository : BaseRepository<ServiceEntity>
    {
        private readonly Context _db;

        public ServiceRepository(Context context) : base(context)
        {
            _db = context;
        }
    }
}