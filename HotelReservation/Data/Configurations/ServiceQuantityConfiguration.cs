using System;
using System.Collections.Generic;
using System.Text;
using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    class ServiceQuantityConfiguration : IEntityTypeConfiguration<ServiceQuantityEntity>
    {
        public void Configure(EntityTypeBuilder<ServiceQuantityEntity> builder)
        {
            builder
                .HasOne(x => x.Order)
                .WithMany(x => x.Services);
        }
    }
}
