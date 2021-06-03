using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HotelReservation.Data.Interfaces
{
    public interface IPasswordHasher
    {
        public string GenerateHash(string password, SHA256 sha256);
    }
}