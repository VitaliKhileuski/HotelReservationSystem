using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Helpers;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;

namespace HotelReservation.Data.Repositories
{
    public class ServiceRepository : BaseRepository<ServiceEntity> , IServiceRepository
    {
        private readonly Context _db;

        public ServiceRepository(Context context) : base(context)
        {
            _db = context;
        }

        public IEnumerable<ServiceEntity> GetFilteredServices(HotelEntity hotel, string sortField, bool ascending)
        {
            var services = hotel.Services;
            if (string.IsNullOrEmpty(sortField))
            {
                return services;
            }

            return services.AsQueryable().OrderByPropertyName(sortField, ascending);
        }
    }
}