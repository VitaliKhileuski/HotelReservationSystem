using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;


namespace HotelReservation.Data.Interfaces
{
    public interface IBaseRepository<TEntity>  where TEntity : Entity
    {
        Task<int> GetCountAsync();

        IEnumerable<TEntity> GetAll();

        Task<TEntity> GetAsync(Guid id);

        IEnumerable<TEntity> Find(int pageNumber, int pageSize);

        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity newEntity);

        Task<TEntity> DeleteAsync(Guid id);
    }
}