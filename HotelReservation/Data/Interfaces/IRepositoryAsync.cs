using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.Data.Interfaces
{
   public interface IRepositoryAsync<T> where T : class
    {
        Task<T> GetAsync(int id);
        Task CreateAsync(T item);
        Task DeleteAsync(int id);
    }
}