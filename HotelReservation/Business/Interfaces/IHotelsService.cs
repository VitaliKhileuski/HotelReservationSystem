﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IHotelsService
    {
        Task AddHotel(HotelModel hotel);
        Task<HotelModel> GetById(Guid id);
        Task UpdateHotelAdmin(Guid hotelId, string userId);
        Task UpdateHotel(Guid hotelId, HotelModel hotel,string userId);
        Task DeleteHotelById(Guid hotelId,string UserId);
        Tuple<List<HotelModel>,int> GetFilteredHotels(DateTime checkInDate, DateTime checkOutDate, string country, string city, Pagination hotelPagination);
        Task<Tuple<IEnumerable<HotelModel>, int>> GetHotelsPage(Pagination hotelPagination);
        Task<ICollection<UserModel>> GetHotelAdmins(Guid hotelId);
        Task DeleteHotelAdmin(Guid hotelId, string userId);
        Task<Tuple<IEnumerable<HotelModel>, int>> GetHotelAdminPages(Pagination hotelPagination, Guid hotelAdminId);
    }
}