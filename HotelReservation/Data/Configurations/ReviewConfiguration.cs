using System;
using System.Collections.Generic;
using System.Text;
using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<ReviewEntity>
    {
        public void Configure(EntityTypeBuilder<ReviewEntity> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .HasMany(x => x.Ratings)
                .WithOne(x => x.Review);
            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Reviews);
            builder
                .HasOne(x => x.Hotel)
                .WithMany(x => x.Reviews);
            builder
                .HasOne(x => x.Order);
        }
    }
}
