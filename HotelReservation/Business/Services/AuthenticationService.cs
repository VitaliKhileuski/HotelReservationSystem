using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
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

        private readonly Context db;
        private readonly IConfiguration cfg;
        private readonly HashPassword hash;
        private readonly ITokenService tokenService;
        private readonly UserRepository _repository;
        private readonly Mapper _requestModelToUserEntity;

        public AuthenticationService(Context context, IConfiguration configuration, HashPassword hashPassword, ITokenService tokenService)
        {
            db = context;
            cfg = configuration;
            hash = hashPassword;
            this.tokenService = tokenService;
            _repository = new UserRepository(db);
            _requestModelToUserEntity =
                new Mapper(new MapperConfiguration(x => x.CreateMap<RegisterUserRequestModel, UserEntity>()));

        }
        public async Task<string> Login(LoginUserRequestModel user)
        {
            var userFromDb = await GetUserFromDb(user.Email);
            if (userFromDb == null)
            {
                throw new NotFoundException("no such user exists");
            }

            if (hash.CheckHash(user.Password, userFromDb.Password))
            {
                return tokenService.BuildToken(cfg["Secrets:secretKey"], user.Email);
            }
            throw new IncorrectPasswordException("password is incorrect");
        }

        public async Task<string> Registration(RegisterUserRequestModel user)
        {
            var dbUser = await GetUserFromDb(user.Email);
            if (dbUser != null)
            {
                throw new BadRequestException("user with that email already exists");

            }

            user.Password = hash.GenerateHash(user.Password, SHA256.Create());
            var userEntity = _requestModelToUserEntity.Map<RegisterUserRequestModel, UserEntity>(user);
            userEntity.RoleId = 2;
            await _repository.CreateAsync(userEntity);
            await db.SaveChangesAsync();

            return  tokenService.BuildToken(cfg["Secrets:secretKey"], user.Email);
        }

        public async Task<UserEntity> GetUserFromDb(string email)
        {
            return await db.Users
                .SingleOrDefaultAsync(x => x.Email.Trim() == email.Trim());
        }
    }
}
