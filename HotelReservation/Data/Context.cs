using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using HotelReservation.Data.Configurations;
using HotelReservation.Data.Interfaces;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace HotelReservation.Data
{
    public class Context : DbContext
    {
        private readonly IPasswordHasher _hashPass;
        public Context(DbContextOptions<Context> options,IPasswordHasher hash)
            : base(options)
        {
            _hashPass = hash;
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<HotelEntity> Hotels { get; set; }
        public DbSet<LocationEntity> Locations { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<ServiceEntity> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.ApplyConfiguration(new UserEntityConfiguration())
                .ApplyConfiguration(new HotelEntityConfiguration())
                .ApplyConfiguration(new LocationEntityConfiguration())
                .ApplyConfiguration(new OrderEntityConfiguration())
                .ApplyConfiguration(new RoomEntityConfiguration())
                .ApplyConfiguration(new RefreshTokenConfiguration())
                .ApplyConfiguration(new ServiceEntityConfiguration())
                .ApplyConfiguration(new RoleEntityConfiguration());
        }

    }
}