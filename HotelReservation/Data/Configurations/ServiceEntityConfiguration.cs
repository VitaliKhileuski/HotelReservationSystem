using System;
using System.Collections.Generic;
using System.Text;
using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    class ServiceEntityConfiguration : IEntityTypeConfiguration<ServiceEntity>
    {
        public void Configure(EntityTypeBuilder<ServiceEntity> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .Property(x => x.Payment)
                .IsRequired();
            builder
                .Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();
            builder
                .HasOne(x => x.Hotel)
                .WithMany(x => x.Services);
            builder
                .HasMany(x => x.Orders)
                .WithMany(x => x.Services);

        }
    }
}
