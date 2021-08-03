using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Interfaces
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        Task<UserEntity> GetAsyncByEmail(string email);
        IEnumerable<UserEntity> GetUsers(HotelEntity hotel);
        IEnumerable<string> GetUsersSurnames(string surname, int limit);
        IEnumerable<string> GetUsersEmails(string email,int limit);
        IEnumerable<UserEntity> GetFilteredUsersBySurname(string surname,string email, string sortedField,bool ascending);
        IEnumerable<string> GetHotelAdminsEmails(string email, int limit);
        IEnumerable<string> GetHotelAdminsSurnames(string surname, int limit);
        IEnumerable<string> GetCustomersSurnames(string surname, int limit);
    }
}