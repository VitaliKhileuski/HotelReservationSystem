﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IHotelsService
    {
        Task AddHotel(HotelModel hotel);
        Task<HotelModel> GetById(int id);
        List<HotelModel> GetAll();
        void UpdateHotelAdmin(int hotelId, int userId);
        Task UpdateHotel(int hotelId, HotelModel hotel,int userId);
    }
}