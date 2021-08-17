using System;
using System.Collections.Generic;
using System.Text;
using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class ReviewCategoryWithRatingEntityConfiguration : IEntityTypeConfiguration<ReviewCategoryWithRatingEntity>
    {
        public void Configure(EntityTypeBuilder<ReviewCategoryWithRatingEntity> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .HasOne(x => x.Category);
            builder
                .HasOne(x => x.Review)
                .WithMany(x => x.Ratings);
        }
    }
}
