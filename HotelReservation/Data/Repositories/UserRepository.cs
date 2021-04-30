using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
   public class UserRepository : IRepository<UserEntity>
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

        public async Task CreateAsync(UserEntity user)
        {
            await db.Users.AddAsync(user);
        }

        public void Delete(int id)
        {
            UserEntity user = db.Users.Find(id);
            if (user != null)
                db.Users.Remove(user);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await db.Users.FindAsync(id);

            if (user != null)
            {
                db.Users.Remove(user);
            }
        }

        public IEnumerable<UserEntity> Find(Func<UserEntity, bool> predicate)
        {
            return db.Users.Include(x => x.Role).Where(predicate).ToList();
        }

        public async Task<IEnumerable<UserEntity>> FindAsync(Func<UserEntity, bool> predicate)
        {
            return await Task.Run((() => Find(predicate)));
        }

        public UserEntity Get(int id)
        {
            return db.Users.Find(id);
        }

        public IEnumerable<UserEntity> GetAll()
        {
            return db.Users;
        }

        public async Task<IEnumerable<UserEntity>> GetAllAsync()
        {
            return await Task.Run(GetAll);
        }

        public async Task<UserEntity> GetAsync(int id)
        {
            return await Task.Run((() => Get(id)));
        }

        public void Update(UserEntity user)
        {
            db.Entry(user).State = EntityState.Modified;
        }

        public async Task UpdateAsync(UserEntity user)
        {
            await Task.Run(() => Update(user));
        }
    }
}
