using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Helpers;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
   public class UserRepository : BaseRepository<UserEntity> , IUserRepository
    {
        private readonly Context _db;

        public UserRepository(Context context) : base(context)
        {
            _db = context;
        }

        public async Task<UserEntity> GetAsyncByEmail(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public  IEnumerable<UserEntity> GetUsers(HotelEntity hotel)
        {
            return _db.Users.Where(x => x.Role.Name != Roles.Admin).Where(x => !x.OwnedHotels.Contains(hotel));
        }

        public  IEnumerable<UserEntity> GetFilteredUsersBySurname(string surname,string email,string sortedField,bool ascending)
        {

            var filteredUsers =  _db.Users.Where(x => (!string.IsNullOrEmpty(surname) &&  x.Surname.StartsWith(surname) || string.IsNullOrEmpty(surname)) &&
                                                      (!string.IsNullOrEmpty(email) && x.Email.StartsWith(email) || string.IsNullOrEmpty(email)));
            if (string.IsNullOrEmpty(sortedField))
            {
                return filteredUsers;
            }

            return filteredUsers.OrderByPropertyName(sortedField, ascending);
        }

        public IEnumerable<string> GetUsersEmails(string email, int limit)
        {

            return _db.Users.Select(x =>x.Email).Where(x => !string.IsNullOrEmpty(email) && x.StartsWith(email) || string.IsNullOrEmpty(email)).Take(limit);
        }

        public IEnumerable<string> GetUsersSurnames(string surname,int limit)
        {
            return _db.Users.Select(x => x.Surname).Distinct().Where(x => !string.IsNullOrEmpty(surname) && x.StartsWith(surname) || string.IsNullOrEmpty(surname)).Take(limit);
        }

        public IEnumerable<string> GetHotelAdminsEmails(string email, int limit)
        {
            return _db.Users.Where(x => x.Role.Name == Roles.HotelAdmin)
                .Select(x => x.Email).Where(x => !string.IsNullOrEmpty(email) && x.StartsWith(email) || string.IsNullOrEmpty(email)).Take(limit);
        }
        public IEnumerable<string> GetHotelAdminsSurnames(string surname, int limit)
        {
            return _db.Users.Where(x => x.Role.Name == Roles.HotelAdmin)
                .Select(x => x.Surname).Distinct().Where(x => !string.IsNullOrEmpty(surname) && x.StartsWith(surname) || string.IsNullOrEmpty(surname)).Take(limit);
        }

        public IEnumerable<string> GetCustomersSurnames(string surname,int limit)
        {
            return _db.Users.Where(x => x.Orders.Count != 0).Select(x => x.Surname).Distinct().Where(x => !string.IsNullOrEmpty(surname) && x.StartsWith(surname) || string.IsNullOrEmpty(surname)).Take(limit);
        }
    }
}