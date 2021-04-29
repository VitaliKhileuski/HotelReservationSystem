using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Threading.Tasks;
using Business.DTO;
using Business.Interfaces;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto.Tls;

namespace Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly Context db;
        private readonly IConfiguration cfg;
        private readonly HashPassword hash;
        private readonly ITokenService tokenService;

        public AuthenticationService(Context context, IConfiguration configuration,HashPassword hashPassword,ITokenService tokenService)
        { 
            db = context;
            cfg = configuration;
            hash = hashPassword;
            this.tokenService = tokenService;

        }
        public async Task<string> Login(LoginUserDTO user)
        {
            var userFromDb = await GetUserFromDb(user);
            if (userFromDb!=null && hash.CheckHash(user.Password, userFromDb.Password))
            {
                return tokenService.BuildToken(cfg["Secrets:secretKey"],user.Email);
            }

            return null;
        }

        public Task<string> Registration(LoginUserDTO user)
        {
            throw new NotImplementedException();
        }

        public async Task<UserEntity> GetUserFromDb(LoginUserDTO user)
        {
            return await db.Users
                .SingleOrDefaultAsync(x => x.Email.Trim() == user.Email.Trim());
        }
    }
}
