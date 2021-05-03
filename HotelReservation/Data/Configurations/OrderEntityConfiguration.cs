using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
   public class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .Property(x => x.DateOrdered)
                .IsRequired();
            builder
                .Property(x => x.StartDate)
                .IsRequired();
            builder
                .Property(x => x.EndDate)
                .IsRequired();
            builder
                .Property(x => x.FullPrice)
                .IsRequired();
            builder
                .HasOne(x => x.Room)
                .WithOne(x => x.Order);
            builder
                .HasOne(x => x.Customer)
                .WithMany(x => x.Orders);
        }
    }
}