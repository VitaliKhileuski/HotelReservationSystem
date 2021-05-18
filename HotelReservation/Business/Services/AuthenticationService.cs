using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Mappers;
using Business.Models;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly Context _db;
        private readonly IConfiguration _cfg;
        private readonly HashPassword _hash;
        private readonly ITokenService _tokenService;
        private readonly UserRepository _repository;
        private readonly Mapper _mapper;

        public AuthenticationService(Context context, IConfiguration configuration, HashPassword hashPassword, ITokenService tokenService, MapConfiguration cfg)
        {
            _db = context;
            _cfg = configuration;
            _hash = hashPassword;
            _tokenService = tokenService;
            _repository = new UserRepository(_db);
            _mapper = new Mapper(cfg.UserConfiguration);
        }
        public async Task<List<string>> Login(LoginUserModel user)
        {
            var userFromDb = await GetUserFromDb(user.Email);
            if (userFromDb == null)
            {
                throw new NotFoundException("no such user exists");
            }

            string token;
            if (_hash.CheckHash(user.Password, userFromDb.Password))
            {
                token = _tokenService.BuildToken(_cfg["Secrets:secretKey"], user.Email, userFromDb.Role.Name, userFromDb.Id);
            }
            else
            {
                throw new IncorrectPasswordException("password is incorrect");
            }

            RefreshTokenEntity refreshToken = new RefreshTokenEntity
            {
                Token = _tokenService.GenerateRefreshToken(),
                User = userFromDb
            };

            if (userFromDb.RefreshToken != null)
            {
                _db.RefreshTokens.Remove(userFromDb.RefreshToken);
            }
            await _db.RefreshTokens.AddAsync(refreshToken);

            await _db.SaveChangesAsync();

            return new List<string> { token, refreshToken.Token };
        }

        public async Task<List<string>> Registration(RegisterUserModel user)
        {
            var dbUser = await GetUserFromDb(user.Email);
            if (dbUser != null)
            {
                throw new BadRequestException("user with that email already exists");
            }
            user.Password = _hash.GenerateHash(user.Password, SHA256.Create());
            var userEntity = _mapper.Map<RegisterUserModel, UserEntity>(user);
            userEntity.RoleId = 2;
            
            var refreshToken = new RefreshTokenEntity
            {
                Token = _tokenService.GenerateRefreshToken(), User = userEntity
            };
            userEntity.RefreshToken = refreshToken;
            await _repository.CreateAsync(userEntity);
            await _db.SaveChangesAsync();
            var userEntityFromDb =  _db.Users.FirstOrDefault(x => x.Email == userEntity.Email);
            var token = _tokenService.BuildToken(_cfg["Secrets:secretKey"], user.Email, "User", userEntityFromDb.Id);
            return new List<string> { token, refreshToken.Token };
        }

        public async Task<UserEntity> GetUserFromDb(string email)
        {
            return await _db.Users
                .SingleOrDefaultAsync(x => x.Email.Trim() == email.Trim());
        }

        public async Task<UserEntity> GetDbUser(TokenModel refreshToken)
        {
            return await _db.
                Users.SingleOrDefaultAsync(x => x.RefreshToken.Token == refreshToken.Token);
        }
        public async Task<List<string>> RefreshTokenVerification(TokenModel refreshToken)
        {
            var dbUser = await GetDbUser(refreshToken);
            if (dbUser == null)
            {
                throw (new BadRequestException("Invalid refresh token."));
            }

            var newRefreshToken = new RefreshTokenEntity
            {
                Token = _tokenService.GenerateRefreshToken(),
                User = dbUser
            };

            if (dbUser.RefreshToken != null)
            {
                _db.RefreshTokens.Remove(dbUser.RefreshToken);
            }

            await _db.RefreshTokens.AddAsync(newRefreshToken);
            await _db.SaveChangesAsync();
            var newJwtToken = _tokenService.BuildToken(_cfg["Secrets:secretKey"], dbUser.Email, dbUser.Role.Name, dbUser.Id);
            return new List<string> { newJwtToken, newRefreshToken.Token };
        }
    }
}