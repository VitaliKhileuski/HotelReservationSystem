using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class ServiceEntityConfiguration : IEntityTypeConfiguration<ServiceEntity>
    {
        public void Configure(EntityTypeBuilder<ServiceEntity> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .Property(x => x.Payment)
                .IsRequired()
                .HasColumnType("decimal(18,4)");
            builder
                .Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();
            builder
                .HasOne(x => x.Hotel)
                .WithMany(x => x.Services);
        }
    }
}