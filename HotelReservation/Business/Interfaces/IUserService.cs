using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IUserService
    {
        Task<ICollection<UserModel>> GetAll(int hotelId);
        Task<UserModel> GetById(int userId);
        Task DeleteById(int userId);
        Task Update(int id, UserModel user); 
        Task AddUser(UserModel user);
    }
}