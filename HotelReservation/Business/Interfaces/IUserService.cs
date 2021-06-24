﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IUserService
    {
        Task<ICollection<UserModel>> GetAll(Guid hotelId);
        Task<UserModel> GetById(Guid userId);
        Task DeleteById(Guid userId);
        Task Update(Guid id, UserModel user); 
        Task AddUser(UserModel user);
    }
}