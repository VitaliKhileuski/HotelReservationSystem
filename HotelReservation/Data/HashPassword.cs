using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto.Utilities;

namespace HotelReservation.Data
{
   public class HashPassword
   {
       private IConfiguration configuration;
       
       public HashPassword(IConfiguration cfg)
       {
           configuration = cfg;
       }

       public bool CheckHash(string password,string hash)
        {
            var currentHash = GenerateHash(password,SHA256.Create());
            return currentHash == hash;
        }

        public  string GenerateHash(string password,SHA256 sha256)
        {
            var salt = configuration.GetSection("Salt").Value;
            var passwordBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            string passwordHash = Convert.ToBase64String(passwordBytes);
            StringBuilder result = new StringBuilder(passwordHash.Length + salt.Length);
            result.Append(passwordHash);
            result.Append(salt);
            return result.ToString();
        }
    }

}
