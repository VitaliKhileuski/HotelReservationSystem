using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
   public class UserRepository : IRepository<UserEntity>, IRepositoryAsync<UserEntity>
    {
        private readonly Context _db;

        public UserRepository(Context context)
        {
            this._db = context;
        }
        public void Create(UserEntity user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public async Task CreateAsync(UserEntity user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            UserEntity user = _db.Users.Find(id);
            if (user != null)
            {
                _db.Users.Remove(user);
            }
            else
            {
                throw new Exception("user with that id not found");
            }
            _db.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user != null)
            {
                _db.Users.Remove(user);
            }
            else
            {
                throw new Exception("user with that id not found");
            }

            await _db.SaveChangesAsync();
        }

        public IEnumerable<UserEntity> Find(Func<UserEntity, bool> predicate)
        {
            return _db.Users.Include(x => x.Role).Where(predicate).ToList();
        }

        public UserEntity Get(int id)
        {
            var user = _db.Users.Find(id);
            if (user == null)
            {
                throw new Exception("user with that id not found");
            }
            return _db.Users.Find(id);
            
        }

        public IEnumerable<UserEntity> GetAll()
        {
            if (!_db.Users.Any())
            {
                throw new Exception("database doesn't contains any users");
            }

            return _db.Users;
        }

        public async Task<UserEntity> GetAsync(int id)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(UserEntity user)
        {
            _db.Entry(user).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}