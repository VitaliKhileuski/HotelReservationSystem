using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IUserService
    {
        public ICollection<UserModel> GetAll();
        public Task<UserModel> GetById(int userId);
        public Task DeleteById(int userId);
        public void Update(int id, UserModel user);
    }
}