using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class RoleRepository : IRepository<RoleEntity>, IRepositoryAsync<RoleEntity>
    {
        private readonly Context _db;

        public RoleRepository(Context context)
        {
            _db = context;
        }
        public void Create(RoleEntity role)
        {
            _db.Roles.Add(role);
            _db.SaveChanges();
        }

        public async Task CreateAsync(RoleEntity role)
        {
            await _db.Roles.AddAsync(role);
            await _db.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            RoleEntity role = _db.Roles.Find(id);
            if (role != null)
            {
                _db.Roles.Remove(role);
            }

            _db.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var role = await _db.Roles.FindAsync(id);

            if (role != null)
            {
                _db.Roles.Remove(role);
            }

            await _db.SaveChangesAsync();
        }

        public IEnumerable<RoleEntity> Find(Func<RoleEntity, bool> predicate)
        {
            return _db.Roles.Where(predicate).ToList();
        }

        public async Task<IEnumerable<RoleEntity>> FindAsync(Func<RoleEntity, bool> predicate)
        {
            return await Task.Run((() => Find(predicate)));
        }

        public RoleEntity Get(int id)
        {
            return _db.Roles.Find(id);
        }

        public IEnumerable<RoleEntity> GetAll()
        {
            return _db.Roles;
        }

        public async Task<IEnumerable<RoleEntity>> GetAllAsync()
        {
            return await Task.Run(GetAll);
        }

        public async Task<RoleEntity> GetAsync(int id)
        {
            return await Task.Run(() => Get(id));
        }

        public void Update(RoleEntity role)
        {
            _db.Entry(role).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public async Task UpdateAsync(RoleEntity role)
        {
            await Task.Run(() => Update(role));
            await _db.SaveChangesAsync();
        }
    }
}