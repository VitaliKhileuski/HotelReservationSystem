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
   public class RoomRepository : IRepository<RoomEntity>
    {
        private readonly Context db;

        public RoomRepository(Context context)
        {
            this.db = context;
        }
        public void Create(RoomEntity room)
        {
            db.Rooms.Add(room);
        }

        public async Task CreateAsync(RoomEntity room)
        {
            await db.Rooms.AddAsync(room);
        }

        public void Delete(int id)
        {
            RoomEntity room = db.Rooms.Find(id);
            if (room != null)
            {
                db.Rooms.Remove(room);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var room = await db.Rooms.FindAsync(id);

            if (room != null)
            {
                db.Rooms.Remove(room);
            }
        }

        public IEnumerable<RoomEntity> Find(Func<RoomEntity, bool> predicate)
        {
            return db.Rooms.Where(predicate).ToList();
        }

        public async Task<IEnumerable<RoomEntity>> FindAsync(Func<RoomEntity, bool> predicate)
        {
            return await Task.Run((() => Find(predicate)));
        }

        public RoomEntity Get(int id)
        {
            return db.Rooms.Find(id);
        }

        public IEnumerable<RoomEntity> GetAll()
        {
            return db.Rooms;
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
            db.Entry(room).State = EntityState.Modified;
        }

        public async Task UpdateAsync(RoomEntity room)
        {
            await Task.Run(() => Update(room));
        }
    }
}
