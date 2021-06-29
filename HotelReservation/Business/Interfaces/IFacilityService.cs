using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IFacilityService
    {
        public ICollection<ServiceModel> GetAllServices();
        public Task<ServiceModel> GetServiceById(Guid serviceId);
        public Task AddServiceToHotel(Guid hotelId, string userId, ServiceModel serviceModel);
        public Task DeleteOrderFromHotel(Guid serviceId, string userId);
        Task<Tuple<IEnumerable<ServiceModel>, int>> GetServicesPage(Guid hotelId, Pagination hotelPagination);
        Task UpdateService(Guid serviceId, string userId, ServiceModel serviceModel);
    }
}