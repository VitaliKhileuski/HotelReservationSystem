using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
   public class RoomRepository : IRepository<RoomEntity>, IRepositoryAsync<RoomEntity>
    {
        private readonly Context _db;

        public RoomRepository(Context context)
        {
            _db = context;
        }
        public void Create(RoomEntity room)
        {
            _db.Rooms.Add(room);
            _db.SaveChanges();
        }

        public async Task CreateAsync(RoomEntity room)
        {
            await _db.Rooms.AddAsync(room);
            await _db.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            RoomEntity room = _db.Rooms.Find(id);
            if (room != null)
            {
                _db.Rooms.Remove(room);
            }
            _db.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var room = await _db.Rooms.FindAsync(id);

            if (room != null)
            {
                _db.Rooms.Remove(room);
            }

            await _db.SaveChangesAsync();
        }

        public IEnumerable<RoomEntity> Find(Func<RoomEntity, bool> predicate)
        {
            return _db.Rooms.Where(predicate).ToList();
        }

        public async Task<IEnumerable<RoomEntity>> FindAsync(Func<RoomEntity, bool> predicate)
        {
            return await Task.Run((() => Find(predicate)));
        }

        public RoomEntity Get(int id)
        {
            return _db.Rooms.Find(id);
        }

        public IEnumerable<RoomEntity> GetAll()
        {
            return _db.Rooms;
        }

        public async Task<IEnumerable<RoomEntity>> GetAllAsync()
        {
            return await Task.Run(GetAll);
        }

        public async Task<RoomEntity> GetAsync(int id)
        {
            return await Task.Run((() => Get(id)));
        }

        public void Update(RoomEntity room)
        {
            _db.Entry(room).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public async Task UpdateAsync(RoomEntity room)
        {
            await Task.Run(() => Update(room));
            await _db.SaveChangesAsync();
        }
    }
}