using System;
using System.Collections.Generic;
using System.Text;
using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .Property(x => x.Token)
                .IsRequired();
            builder
                .HasOne(x => x.User)
                .WithOne(x => x.RefreshToken)
                .HasForeignKey<UserEntity>(x => x.RefreshTokenId);
        }
    }
}