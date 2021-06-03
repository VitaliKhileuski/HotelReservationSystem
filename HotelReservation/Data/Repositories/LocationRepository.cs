using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
   public  class LocationRepository 
    {
        private readonly Context _db;

        public LocationRepository(Context context)
        {
            _db = context;
        }

        public async Task CreateAsync(LocationEntity location)
        {
            await _db.Locations.AddAsync(location);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var location = await _db.Locations.FindAsync(id);
            if (location!=null)
            {
                _db.Locations.Remove(location);
            }

            await _db.SaveChangesAsync();
        }

        public IEnumerable<LocationEntity> Find(Func<LocationEntity, bool> predicate)
        {
            return _db.Locations.Where(predicate).ToList();
        }

        public IEnumerable<LocationEntity> GetAll()
        {
            return _db.Locations;
        }

        public async Task<LocationEntity> GetAsync(int id)
        {
            return await _db.Locations.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(LocationEntity location)
        {
            _db.Entry(location).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}