using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Interfaces
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        Task<UserEntity> GetAsyncByEmail(string email);
        IEnumerable<UserEntity> GetUsers(HotelEntity hotel);
        IEnumerable<string> GetUsersSurnames();
        IEnumerable<string> GetUsersEmails();
        IEnumerable<UserEntity> GetFilteredUsersBySurname(string surname);
        IEnumerable<string> GetHotelAdminsEmails();
        IEnumerable<string> GetHotelAdminsSurnames();
        IEnumerable<string> GetCustomersSurnames();
    }
}