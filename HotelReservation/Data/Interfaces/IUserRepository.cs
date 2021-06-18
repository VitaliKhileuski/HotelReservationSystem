using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Interfaces
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        Task<UserEntity> GetAsyncByEmail(string email);
        IEnumerable<UserEntity> GetUsers(HotelEntity hotel);
    }
}
