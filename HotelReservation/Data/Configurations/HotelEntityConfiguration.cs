﻿using System;
using System.Collections.Generic;
using System.Text;
using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    class HotelEntityConfiguration : IEntityTypeConfiguration<HotelEntity>
    {
        public void Configure(EntityTypeBuilder<HotelEntity> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder
                .HasOne(x => x.Location)
                .WithOne(x => x.Hotel)
                .HasForeignKey<LocationEntity>(x => x.HotelId);
            builder.HasMany(x => x.Rooms)
                .WithOne(x => x.Hotel)
                .HasForeignKey(x => x.HotelId);

        }
    }
}