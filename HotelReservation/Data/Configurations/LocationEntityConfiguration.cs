using System;
using System.Collections.Generic;
using System.Text;
using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    class LocationEntityConfiguration : IEntityTypeConfiguration<LocationEntity>
    {
        public void Configure(EntityTypeBuilder<LocationEntity> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .Property(x => x.Country)
                .IsRequired()
                .HasMaxLength(40);
            builder
                .Property(x => x.Region)
                .IsRequired()
                .HasMaxLength(50);
            builder
                .Property(x => x.City)
                .HasMaxLength(100);
            builder
                .Property(x => x.BuildingNumber)
                .IsRequired()
                .HasMaxLength(10);
            builder.Property(x => x.Street)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasOne(x => x.Hotel)
                .WithOne(x => x.Location);

        }
    }
}
