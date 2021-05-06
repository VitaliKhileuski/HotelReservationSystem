using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.Data.Interfaces
{
   public interface IRepositoryAsync<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate);
        Task CreateAsync(T item);
        Task UpdateAsync(T newItem);
        Task DeleteAsync(int id);
    }
}