using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Helpers;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace HotelReservation.Data.Repositories
{
    public class HotelRepository : BaseRepository<HotelEntity> , IHotelRepository
    {
        private readonly Context _db;

        public HotelRepository(Context context) : base(context)
        {
            _db = context;
        }
        public IEnumerable<HotelEntity> GetHotelAdminsHotels(int pageNumber, int pageSize, UserEntity hotelAdmin)
        {
            return _db.Hotels.Skip((pageNumber - 1) * pageSize).Take(pageSize).Where(x => x.Admins.Contains(hotelAdmin));
        }
        public async Task<int> GetHotelAdminsHotelsCount(UserEntity hotelAdmin)
        {
            return await _db.Hotels.Where(x => x.Admins.Contains(hotelAdmin)).CountAsync();
        }

        public IEnumerable<string> GetHotelNames()
        {
            return _db.Hotels.Select(x => x.Name).Distinct();
        }

        public IEnumerable<HotelEntity> GetFilteredHotels(string country, string city, string hotelName, string email, string surname,string sortField,bool ascending)
        {
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(surname))
            {
                
                var filteredHotels = _db.Hotels.Where(x => (!string.IsNullOrEmpty(country) && x.Location.Country == country || string.IsNullOrEmpty(country)) &&
                         (!string.IsNullOrEmpty(city) && x.Location.City == city || string.IsNullOrEmpty(city)) && (!string.IsNullOrEmpty(hotelName) &&
                                                                               x.Name == hotelName || string.IsNullOrEmpty(hotelName)));
                return string.IsNullOrEmpty(sortField) ? filteredHotels : filteredHotels.OrderByPropertyName(sortField, ascending);
            }
            var hotels = new List<HotelEntity>();
            if (!string.IsNullOrEmpty(email))
            {
                var userEntity =  _db.Users.Where(x => x.Email == email);
                hotels.AddRange(userEntity.SelectMany(x => x.OwnedHotels));
            }

            if (!string.IsNullOrEmpty(surname))
            {
                var users = _db.Users.Where(x => x.Surname == surname);
                hotels.AddRange(users.SelectMany(x => x.OwnedHotels));
            }

            var filteredHotelAdminHotels =  hotels.Where(x => (!string.IsNullOrEmpty(country) && x.Location.Country == country || string.IsNullOrEmpty(country))
                                                                            && ( !string.IsNullOrEmpty(city) && x.Location.City == city || string.IsNullOrEmpty(city)) &&
                                                                            (!string.IsNullOrEmpty(hotelName) &&
                                                                            x.Name == hotelName || string.IsNullOrEmpty(hotelName))).AsQueryable();
            return string.IsNullOrEmpty(sortField) ? filteredHotelAdminHotels : filteredHotelAdminHotels.OrderByPropertyName(sortField, ascending);
        }
    }
}