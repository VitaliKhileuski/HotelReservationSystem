using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using HotelReservation.Data.Configurations;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

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
        public DbSet<LocationEntity> Locations { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            

            modelBuilder.ApplyConfiguration(new UserEntityConfiguration())
                .ApplyConfiguration(new HotelEntityConfiguration())
                .ApplyConfiguration(new LocationEntityConfiguration())
                .ApplyConfiguration(new OrderEntityConfiguration())
                .ApplyConfiguration(new RoomEntityConfiguration());

            
        }

    }
}
