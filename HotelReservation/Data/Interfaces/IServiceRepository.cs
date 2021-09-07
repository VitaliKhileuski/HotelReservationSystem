using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Interfaces
{
    public interface IServiceRepository : IBaseRepository<ServiceEntity>
    {
        IEnumerable<ServiceEntity> GetFilteredServices(HotelEntity hotel,string sortField,bool ascending);
    }
}