﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;
using Business.Models.FilterModels;

namespace Business.Interfaces
{
    public interface IUserService
    {
        Task<ICollection<UserModel>> GetAll(Guid hotelId);
        Task<UserModel> GetById(Guid id,string userId);
        Task DeleteById(Guid userId);
        Task<string> Update(Guid id,string userId, UserModel user); 
        Task AddUser(UserModel user);
        IEnumerable<string> GetUsersSurnames();
        public IEnumerable<string> GetUsersEmails();
        public IEnumerable<string> GetHotelAdminsEmails();
        public IEnumerable<string> GetHotelAdminsSurnames();
        public IEnumerable<string> GetCustomersSurnames();
        Task<PageInfo<UserModel>> GetUsersPage(UserFilter userFilter,string userId, Pagination pagination,SortModel sortModel);
        
    }
}