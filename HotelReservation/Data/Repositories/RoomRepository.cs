using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    class RoomRepository : IRepository<RoomEntity>
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

        public void Delete(int id)
        {
            RoomEntity room = db.Rooms.Find(id);
            if (room != null)
            {
                db.Rooms.Remove(room);
            }
        }

        public IEnumerable<RoomEntity> Find(Func<RoomEntity, bool> predicate)
        {
            return db.Rooms.Where(predicate).ToList();
        }

        public RoomEntity Get(int id)
        {
            return db.Rooms.Find(id);
        }

        public IEnumerable<RoomEntity> GetAll()
        {
            return db.Rooms;
        }

        public void Update(RoomEntity room)
        {
            db.Entry(room).State = EntityState.Modified;
        }
    }
}
