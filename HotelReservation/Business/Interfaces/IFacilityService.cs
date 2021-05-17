using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IFacilityService
    {
        public ICollection<ServiceModel> GetAllServices();
        public Task<ServiceModel> GetServiceById(int serviceId);
        public Task AddServiceToHotel(int hotelId, int userId, ServiceModel serviceModel);
        public Task DeleteOrderFromHotel(int serviceId, int userId);
    }
}