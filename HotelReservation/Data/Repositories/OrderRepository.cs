using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class OrderRepository : BaseRepository<OrderEntity>
    {
        private readonly Context _db;

        public OrderRepository(Context context) : base(context)
        {
            _db = context;
        }
    }
}