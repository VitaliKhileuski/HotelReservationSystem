﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;

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
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(ILogger<AuthenticationService> logger, Context context, IConfiguration configuration, HashPassword hashPassword, ITokenService tokenService, MapConfiguration cfg)
        {
            _db = context;
            _cfg = configuration;
            _hash = hashPassword;
            _tokenService = tokenService;
            _repository = new UserRepository(_db);
            _mapper = new Mapper(cfg.UserConfiguration);
            _logger = logger;
        }
        public async Task<List<string>> Login(LoginUserModel user)
        {
            var userFromDb = await GetUserFromDb(user.Email);
            if (userFromDb == null)
            {
                _logger.LogError("no such user exists");
                throw new NotFoundException("no such user exists");
            }

            string token;
            if (_hash.CheckHash(user.Password, userFromDb.Password))
            {
                token = _tokenService.BuildToken(_cfg["Secrets:secretKey"], user.Email, userFromDb.Role.Name,userFromDb.Name,userFromDb.Id);
            }
            else
            {
                _logger.LogError("password is incorrect");
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
                _logger.LogError("user with that email already exists");
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
            var userEntityFromDb = _db.Users.FirstOrDefault(x => x.Email == userEntity.Email);
            var token = _tokenService.BuildToken(_cfg["Secrets:secretKey"], user.Email, "User",user.Name,userEntityFromDb.Id);
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
                _logger.LogError("Invalid refresh token");
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
            var newJwtToken = _tokenService.BuildToken(_cfg["Secrets:secretKey"], dbUser.Email, dbUser.Role.Name,dbUser.Name, dbUser.Id);
            return new List<string> { newJwtToken, newRefreshToken.Token };
        }
        public void JWTTokenVerification(string token)
        {

            var tokenParts = token.Split('.');
            var key = Encoding.ASCII.GetBytes(_cfg["Secrets:secretKey"]);
            var PartsInBytes = Encoding.ASCII.GetBytes(tokenParts[0]+'.'+tokenParts[1]);
            var hash = new HMACSHA256(key);
            var thirdPartInbytes = hash.ComputeHash(PartsInBytes);
            var thirdPart = Convert.ToBase64String(thirdPartInbytes);
            var tokenPartsBytes =  Encoding.ASCII.GetBytes(tokenParts[2]);
            if (tokenParts[2] != thirdPart)
            {
                _logger.LogError("problems with token verification");
                throw new BadRequestException("problems with token verification");
            }
        }
    }
}