using System;
using System.Collections.Generic;
using System.Text;
using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class AverageReviewCategoryRatingsConfiguration : IEntityTypeConfiguration<AverageReviewCategoryRatingsEntity>
    {
        public void Configure(EntityTypeBuilder<AverageReviewCategoryRatingsEntity> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .HasOne(x => x.Category);
            builder
                .HasOne(x => x.Hotel)
                .WithMany(x => x.AverageCategoryRatings);
        }
    }
}
