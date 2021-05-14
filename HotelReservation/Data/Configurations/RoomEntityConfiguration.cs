﻿using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
   public class RoomEntityConfiguration : IEntityTypeConfiguration<RoomEntity>
    {
        public void Configure(EntityTypeBuilder<RoomEntity> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .Property(x => x.RoomNumber)
                .HasMaxLength(10)
                .IsRequired();
            builder.HasOne(x => x.Hotel)
                .WithMany(x => x.Rooms)
                .HasForeignKey(x => x.HotelId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Rooms)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(x => x.Order)
                .WithOne(x => x.Room)
                .HasForeignKey<OrderEntity>(x => x.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}