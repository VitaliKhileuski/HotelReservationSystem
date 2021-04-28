using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using HotelReservation.Data.Configurations;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;
using Org.BouncyCastle.Crypto.Generators;

namespace HotelReservation.Data
{
    public class Context : DbContext
    {
        private HashPassword hashPass;
        public Context(DbContextOptions<Context> options,HashPassword hash)
            : base(options)
        {
            hashPass = hash;
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
            modelBuilder.Entity<RoleEntity>().HasData(new RoleEntity()
            {
                Id = 1,
                Name = "Admin",
                Users = null,
            });
            modelBuilder.Entity<UserEntity>().HasData(new UserEntity()
            {
                Id = 1,
                RoleId = 1,
                Birthdate = new DateTime(2000,10,10),
                Name = "Admin",
                Password = hashPass.GenerateHash("Admin", SHA256.Create()),
                Surname = "Admin",
                PhoneNumber = "+375297809088"
            });

        }

    }
}
