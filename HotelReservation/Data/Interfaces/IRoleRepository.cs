using System.Threading.Tasks;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Interfaces
{
    public interface IRoleRepository : IBaseRepository<RoleEntity>
    {
        Task<RoleEntity> GetAsyncByName(string name);
    }
}