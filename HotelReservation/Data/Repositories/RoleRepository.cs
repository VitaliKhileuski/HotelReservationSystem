using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class RoleRepository : BaseRepository<RoleEntity>, IRoleRepository
    {
        private readonly Context _db;

        public RoleRepository(Context context) : base(context)
        {
            _db = context;
        }

        public async Task<RoleEntity> GetAsyncByName(string name)
        {
            return await _db.Roles.FirstOrDefaultAsync(x => x.Name==name);
        }
    }
}