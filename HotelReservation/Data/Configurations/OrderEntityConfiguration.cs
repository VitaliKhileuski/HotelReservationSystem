using System;
using System.Collections.Generic;
using System.Text;
using HotelReservation.Data.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .Property(x => x.DateOrdered)
                .IsRequired();
            builder
                .Property(x => x.StartDate)
                .IsRequired();
            builder
                .Property(x => x.EndDate)
                .IsRequired();
            builder
                .Property(x => x.FullPrice)
                .IsRequired();
            builder
                .HasOne(x => x.Room)
                .WithOne(x => x.Order)
                .HasForeignKey<RoomEntity>(x => x.OrderId);
            builder
                .HasOne(x => x.Customer)
                .WithMany(x => x.Orders);
           
        }
    }
}
