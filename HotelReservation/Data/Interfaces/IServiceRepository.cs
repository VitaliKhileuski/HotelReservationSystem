using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Interfaces
{
    public interface IServiceRepository : IBaseRepository<ServiceEntity>
    {
        IEnumerable<ServiceEntity> GetServicesPageFromHotel(int pageNumber, int pageSize, Guid hotelId);
        Task<int> GetServiceCount(Guid hotelId);
    }
}