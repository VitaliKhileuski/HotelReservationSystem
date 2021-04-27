using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.Data.Interfaces
{
   public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T Get(int id);
        Task<T> GetAsync(int id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate);
        void Create(T item);
        Task CreateAsync(T item);
        void Update(T item);
        Task UpdateAsync(T newItem);
        void Delete(int id);
        Task DeleteAsync(int id);
    }
}
