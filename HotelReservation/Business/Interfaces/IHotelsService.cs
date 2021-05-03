using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Business.Models.RequestModels;

namespace Business.Interfaces
{
    public interface IHotelsService
    {
        Task AddHotel(HotelRequestModel hotel);

    }
}
