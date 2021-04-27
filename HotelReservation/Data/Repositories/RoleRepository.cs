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
    public class RoleRepository : IRepository<RoleEntity>
    {
        private readonly Context db;

        public RoleRepository(Context context)
        {
            db = context;
        }
        public void Create(RoleEntity role)
        {
            db.Roles.Add(role);
        }

        public async Task CreateAsync(RoleEntity role)
        {
            await db.Roles.AddAsync(role);
        }

        public void Delete(int id)
        {
            RoleEntity role = db.Roles.Find(id);
            if (role != null)
            {
                db.Roles.Remove(role);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var role = await db.Roles.FindAsync(id);

            if (role != null)
            {
                db.Roles.Remove(role);
            }
        }

        public IEnumerable<RoleEntity> Find(Func<RoleEntity, bool> predicate)
        {
            return db.Roles.Where(predicate).ToList();
        }

        public async Task<IEnumerable<RoleEntity>> FindAsync(Func<RoleEntity, bool> predicate)
        {
            return await Task.Run((() => Find(predicate)));
        }

        public RoleEntity Get(int id)
        {
            return db.Roles.Find(id);
        }

        public IEnumerable<RoleEntity> GetAll()
        {
            return db.Roles;
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
            db.Entry(role).State = EntityState.Modified;
        }

        public async Task UpdateAsync(RoleEntity role)
        {
            await Task.Run(() => Update(role));
        }
    }
}
