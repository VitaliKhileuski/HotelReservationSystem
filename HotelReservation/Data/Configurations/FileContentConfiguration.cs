using System;
using System.Collections.Generic;
using System.Text;
using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    class FileContentConfiguration : IEntityTypeConfiguration<FileContentEntity>
    {
        public void Configure(EntityTypeBuilder<FileContentEntity> builder)
        {
            builder
                .HasOne(x => x.Attachment)
                .WithOne(x => x.FileContent)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
