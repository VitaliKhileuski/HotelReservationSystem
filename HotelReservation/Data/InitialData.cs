using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Org.BouncyCastle.OpenSsl;

namespace HotelReservation.Data
{
    public class InitialData
    {
        private readonly Context _db;
        private readonly IPasswordHasher _passwordHasher;

        public InitialData(Context context, IPasswordHasher passwordHasher)
        {
            _db = context;
            _passwordHasher = passwordHasher;
        }
        public void InitializeContext()
        {
            if (_db == null)
            {
                return;
            }
            var adminRole = _db.Roles.FirstOrDefault(r => r.Name == Roles.Admin);
            if (adminRole == null)
            {
                _db.Roles.Add(new RoleEntity(Roles.Admin));
            }

            var hotelAdminRole = _db.Roles.FirstOrDefault(r => r.Name == Roles.HotelAdmin);
            if (hotelAdminRole == null)
            {
                _db.Roles.Add(new RoleEntity(Roles.HotelAdmin));
            }

            var userRole = _db.Roles.FirstOrDefault(r => r.Name == Roles.User);
            if (userRole == null)
            {
                _db.Roles.Add(new RoleEntity(Roles.User));
            }

            _db.SaveChanges();

            var admin = _db?.Users.FirstOrDefault(u => u.Email == AppAdmin.Email);
            if (admin == null)
            {
                var role = _db.Roles.FirstOrDefault(x => x.Name == AppAdmin.RolenName);
                if (role == null)
                {
                    return;
                }
                _db.Users.Add(
                    new UserEntity
                    {
                        Email = AppAdmin.Email,
                        Password = _passwordHasher.GenerateHash(AppAdmin.Password, SHA256.Create()),
                        Name = AppAdmin.Name,
                        Surname =  AppAdmin.Surname,
                        RoleId = role.Id,
                        PhoneNumber = AppAdmin.PhoneNumber
                            
                    }
                );
            }
            _db.SaveChanges();

        }
    }
}
