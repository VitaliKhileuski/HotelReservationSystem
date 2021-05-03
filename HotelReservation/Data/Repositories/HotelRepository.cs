using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class HotelRepository : IRepository<HotelEntity>, IRepositoryAsync<HotelEntity>
    {
        private readonly Context _db;

        public HotelRepository(Context context)
        {
            _db = context;
        }

        public void Create(HotelEntity hotel)
        {
            _db.Hotels.Add(hotel);
        }

        public async Task CreateAsync(HotelEntity hotel)
        {
            await _db.Hotels.AddAsync(hotel);
        }

        public void Delete(int id)
        {
            HotelEntity hotel = _db.Hotels.Find(id);
            if (hotel != null)
            {
                _db.Hotels.Remove(hotel);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var hotel = await _db.Hotels.FindAsync(id);

            if (hotel != null)
            {
                _db.Hotels.Remove(hotel);
            }
        }

        public IEnumerable<HotelEntity> Find(Func<HotelEntity, bool> predicate)
        {
            return _db.Hotels.Where(predicate).ToList();
        }

        public async Task<IEnumerable<HotelEntity>> FindAsync(Func<HotelEntity, bool> predicate)
        {
            return await Task.Run(() => Find(predicate));
        }

        public HotelEntity Get(int id)
        {
            return _db.Hotels.Find(id);
        }

        public IEnumerable<HotelEntity> GetAll()
        {
            return _db.Hotels;
        }

        public async Task<IEnumerable<HotelEntity>> GetAllAsync()
        {
            return await Task.Run(GetAll);
        }

        public async Task<HotelEntity> GetAsync(int id)
        {
            return await Task.Run(() => Get(id));
        }

        public void Update(HotelEntity hotel)
        {
            _db.Entry(hotel).State = EntityState.Modified;
        }

        public async Task UpdateAsync(HotelEntity newItem)
        {
            await Task.Run(() => Update(newItem));
        }
    }
}