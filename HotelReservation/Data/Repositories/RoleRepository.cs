using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    class RoleRepository : IRepository<RoleEntity>
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

        public void Delete(int id)
        {
            RoleEntity role = db.Roles.Find(id);
            if (role != null)
            {
                db.Roles.Remove(role);
            }
        }

        public IEnumerable<RoleEntity> Find(Func<RoleEntity, bool> predicate)
        {
            return db.Roles.Where(predicate).ToList();
        }

        public RoleEntity Get(int id)
        {
            return db.Roles.Find(id);
        }

        public IEnumerable<RoleEntity> GetAll()
        {
            return db.Roles;
        }

        public void Update(RoleEntity role)
        {
            db.Entry(role).State = EntityState.Modified;
        }
    }
}
