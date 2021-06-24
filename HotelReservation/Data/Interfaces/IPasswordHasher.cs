using System.Security.Cryptography;

namespace HotelReservation.Data.Interfaces
{
    public interface IPasswordHasher
    {
        public string GenerateHash(string password, SHA256 sha256);
    }
}