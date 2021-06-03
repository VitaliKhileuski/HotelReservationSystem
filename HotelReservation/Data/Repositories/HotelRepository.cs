using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class HotelRepository 
    {
        private readonly Context _db;

        public HotelRepository(Context context)
        {
            _db = context;
        }

        public async Task CreateAsync(HotelEntity hotel)
        {
            await _db.Hotels.AddAsync(hotel);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hotel = await _db.Hotels.FindAsync(id);

            if (hotel != null)
            {
                _db.Hotels.Remove(hotel);
            }

            await _db.SaveChangesAsync();
        }

        public IEnumerable<HotelEntity> Find(Func<HotelEntity, bool> predicate)
        {
            return _db.Hotels.Where(predicate).ToList();
        }

        public IEnumerable<HotelEntity> GetAll()
        {
            return _db.Hotels;
        }

        public async Task<HotelEntity> GetAsync(int id)
        {
            return await _db.Hotels.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Tuple<List<HotelEntity>,int> GetPage(int pageNumber, int pageSize)
        {
            int numberOfHotels = _db.Hotels.Count();
            var pagedData =  _db.Hotels
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Tuple.Create(pagedData,numberOfHotels);
        }

        public void Update(HotelEntity hotel)
        {
            _db.Entry(hotel).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}