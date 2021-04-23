using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<HotelEntity> Hotels { get; set; }
        public DbSet<CountryEntity> Countries { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }

    }
}
