using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    class UserRepository : IRepository<UserEntity>
    {
        private Context db;

        public UserRepository(Context context)
        {
            this.db = context;
        }
        public void Create(UserEntity user)
        {
            db.Users.Add(user);
        }

        public void Delete(int id)
        {
            UserEntity user = db.Users.Find(id);
            if (user != null)
                db.Users.Remove(user);
        }

        public IEnumerable<UserEntity> Find(Func<UserEntity, bool> predicate)
        {
            return db.Users.Where(predicate).ToList();
        }

        public UserEntity Get(int id)
        {
            return db.Users.Find(id);
        }

        public IEnumerable<UserEntity> GetAll()
        {
            return db.Users;
        }

        public void Update(UserEntity user)
        {
            db.Entry(user).State = EntityState.Modified;
        }
    }
}
