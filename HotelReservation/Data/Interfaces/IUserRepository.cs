using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Interfaces
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        Task<UserEntity> GetAsyncByEmail(string email);
        IEnumerable<UserEntity> GetUsers(HotelEntity hotel);
        IEnumerable<string> GetUsersSurnames(string surname);
        IEnumerable<string> GetUsersEmails(string email);
        IEnumerable<UserEntity> GetFilteredUsersBySurname(string surname,string email, string sortedField,bool ascending);
        IEnumerable<string> GetHotelAdminsEmails(string email);
        IEnumerable<string> GetHotelAdminsSurnames(string surname);
        IEnumerable<string> GetCustomersSurnames(string surname);
    }
}