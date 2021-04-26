using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    class HotelRepository : IRepository<HotelEntity>
    {
        private readonly Context db;

        public HotelRepository(Context context)
        {
            this.db = context;
        }

        public void Create(HotelEntity hotel)
        {
            db.Hotels.Add(hotel);
        }

        public void Delete(int id)
        {
            HotelEntity hotel = db.Hotels.Find(id);
            if (hotel != null)
            {
                db.Hotels.Remove(hotel);
            }
        }

        public IEnumerable<HotelEntity> Find(Func<HotelEntity, bool> predicate)
        {
            return db.Hotels.Where(predicate).ToList();
        }

        public HotelEntity Get(int id)
        {
            return db.Hotels.Find(id);
        }

        public IEnumerable<HotelEntity> GetAll()
        {
            return db.Hotels;
        }

        public void Update(HotelEntity hotel)
        {
            db.Entry(hotel).State = EntityState.Modified;
        }
    }
}
