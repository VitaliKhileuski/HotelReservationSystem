using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Interfaces
{
    public interface IHotelsService
    {
        Task AddHotel(HotelModel hotel);
        Task<HotelModel> GetById(int id);
        List<HotelModel> GetAll();
    }
}
