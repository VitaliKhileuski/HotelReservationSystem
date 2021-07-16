using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

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
        Task<PageInfo<UserModel>> GetUsersPage(string email, string surname,string userId, Pagination pagination);
        
    }
}