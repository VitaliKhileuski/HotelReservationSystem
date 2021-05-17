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
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.Surname)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15);
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
        }
    }
}