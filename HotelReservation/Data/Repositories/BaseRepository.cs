using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public  class BaseRepository<TEntity> : IBaseRepository<TEntity>  where TEntity : Entity
    {
        private readonly Context _db;

        protected BaseRepository(Context context)
        {
            _db = context;
            DbSet = context.Set<TEntity>();
        }

        protected DbSet<TEntity> DbSet { get; }

        public async Task<int> GetCountAsync()
        {
            return await DbSet.CountAsync();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DbSet;
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(entity => entity.Id.Equals(id));
        }

        public IEnumerable<TEntity> Find(int pageNumber, int pageSize)
        {
            return DbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var addedEntityEntry = await DbSet.AddAsync(entity);
            await _db.SaveChangesAsync();

            return addedEntityEntry.Entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity newEntity)
        {
            var updatedEntityEntry = DbSet.Update(newEntity);
            await _db.SaveChangesAsync();

            return updatedEntityEntry.Entity;
        }

        public async Task<TEntity> DeleteAsync(int id)
        {
            var deletingEntity = await DbSet.FindAsync(id);

            if (deletingEntity != null)
            {
                DbSet.Remove(deletingEntity);
                await _db.SaveChangesAsync();
            }

            return deletingEntity;
        }
    }
}
