using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    class LocationRepository : IRepository<LocationEntity>
    {
        private readonly Context db;

        public LocationRepository(Context context)
        {
            this.db = context;
        }

        public void Create(LocationEntity location)
        {
            db.Locations.Add(location);
        }

        public void Delete(int id)
        {
            LocationEntity location = db.Locations.Find(id);
            if (location != null)
            {
                db.Locations.Remove(location);
            }
        }

        public IEnumerable<LocationEntity> Find(Func<LocationEntity, bool> predicate)
        {
            return db.Locations.Where(predicate).ToList();
        }

        public LocationEntity Get(int id)
        {
            return db.Locations.Find(id);
        }

        public IEnumerable<LocationEntity> GetAll()
        {
            return db.Locations;
        }

        public void Update(LocationEntity location)
        {
            db.Entry(location).State = EntityState.Modified;
        }
    }
}
