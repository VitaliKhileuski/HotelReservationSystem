using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder
                .Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder
                .Property(x => x.Name)
                .HasDefaultValue("user")
                .HasMaxLength(100);
            builder.Property(x => x.Surname)
                .HasDefaultValue("user")
                .HasMaxLength(200);
            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(15);
            builder
                .Property(x => x.IsVerified)
                .HasDefaultValue(false);
            builder.
                HasOne(x => x.Role)
                .WithMany(x => x.Users);
            builder
                .HasMany(x => x.Orders)
                .WithOne(x => x.Customer)
                .HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.RefreshToken)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.OwnedHotels)
                .WithMany(x => x.Admins);
            builder.HasMany(x => x.Reviews)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}