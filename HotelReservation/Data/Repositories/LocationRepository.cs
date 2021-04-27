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
   public  class LocationRepository : IRepository<LocationEntity>
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

        public async Task CreateAsync(LocationEntity location)
        {
            await db.Locations.AddAsync(location);
        }

        public void Delete(int id)
        {
            LocationEntity location = db.Locations.Find(id);
            if (location != null)
            {
                db.Locations.Remove(location);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var location = await db.Locations.FindAsync(id);
            if (location!=null)
            {
                db.Locations.Remove(location);
            }
        }

        public IEnumerable<LocationEntity> Find(Func<LocationEntity, bool> predicate)
        {
            return db.Locations.Where(predicate).ToList();
        }

        public Task<IEnumerable<LocationEntity>> FindAsync(Func<LocationEntity, bool> predicate)
        {
            return Task.Run(() => Find(predicate));
        }

        public LocationEntity Get(int id)
        {
            return db.Locations.Find(id);
        }

        public IEnumerable<LocationEntity> GetAll()
        {
            return db.Locations;
        }

        public async Task<IEnumerable<LocationEntity>> GetAllAsync()
        {
           return await Task.Run(GetAll);
        }

        public async Task<LocationEntity> GetAsync(int id)
        {
            return await Task.Run(() => Get(id));
        }

        public void Update(LocationEntity location)
        {
            db.Entry(location).State = EntityState.Modified;
        }

        public async Task UpdateAsync(LocationEntity location)
        {
            await Task.Run(() => Update(location));
        }
    }
}
