using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
   public class RoomEntityConfiguration : IEntityTypeConfiguration<RoomEntity>
    {
        public void Configure(EntityTypeBuilder<RoomEntity> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .Property(x => x.RoomNumber)
                .HasMaxLength(10)
                .IsRequired();
            builder
                .Property(x => x.PaymentPerDay)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            builder.HasOne(x => x.Hotel)
                .WithMany(x => x.Rooms)
                .HasForeignKey(x => x.HotelId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Rooms)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasMany(x => x.Order)
                .WithOne(x => x.Room)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasMany(x => x.Attachments)
                .WithOne(x => x.Room)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}