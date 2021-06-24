using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Interfaces
{
    public interface IServiceRepository : IBaseRepository<ServiceEntity>
    {
        IEnumerable<ServiceEntity> GetServicesPageFromHotel(int pageNumber, int pageSize, int hotelId);
        Task<int> GetServiceCount(int hotelId);
    }
}