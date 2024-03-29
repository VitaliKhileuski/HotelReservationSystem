﻿using System.Collections.Generic;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Interfaces;

namespace Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly Context _db;
        private readonly IPasswordHasher _hash;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(ILogger<AuthenticationService> logger, Context context, IPasswordHasher hashPassword, ITokenService tokenService,
            MapConfiguration cfg, IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
        {
            _db = context;
            _hash = hashPassword;
            _tokenService = tokenService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
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
            if (CheckHash(user.Password, userFromDb.Password))
            {
                token = _tokenService.BuildToken(user.Email, userFromDb.Role.Name,userFromDb.Name,userFromDb.Id);
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
            user.Name ??= "User";
            user.Password ??= "testtest1";
            user.Surname ??= "User";
            var token  = String.Empty;
            var dbUser = await GetUserFromDb(user.Email);
            if (dbUser != null)
            {
                _logger.LogError("user with that email already exists");
                throw new BadRequestException("user with that email already exists");
            }
            user.Password = _hash.GenerateHash(user.Password, SHA256.Create());
            var userEntity = _mapper.Map<RegisterUserModel, UserEntity>(user);
            var role = await  _roleRepository.GetAsyncByName(Roles.User);
            userEntity.RoleId = role.Id;
            
            var refreshToken = new RefreshTokenEntity
            {
                Token = _tokenService.GenerateRefreshToken(), User = userEntity
            };
            userEntity.RefreshToken = refreshToken;
            await _userRepository.CreateAsync(userEntity);
            var userEntityFromDb = _db.Users.FirstOrDefault(x => x.Email == userEntity.Email);
            if (userEntityFromDb != null)
            { 
                token = _tokenService.BuildToken(user.Email, Roles.User, user.Name, userEntityFromDb.Id);
            }
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
            var newJwtToken = _tokenService.BuildToken(dbUser.Email, dbUser.Role.Name,dbUser.Name, dbUser.Id);
            return new List<string> { newJwtToken, newRefreshToken.Token };
        }
        private bool CheckHash(string password, string hash)
        {
            var currentHash = _hash.GenerateHash(password, SHA256.Create());
            return currentHash == hash;
        }

        public async Task<bool> IsPasswordCorrect(string userId,string password)
        {
            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }

            return userEntity.Password == _hash.GenerateHash(password,SHA256.Create());
        }
    }
}