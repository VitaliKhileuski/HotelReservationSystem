using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace HotelReservation.Data.Configurations
{
    public class AttachmentEntityConfiguration : IEntityTypeConfiguration<AttachmentEntity>

    {
        public void Configure(EntityTypeBuilder<AttachmentEntity> builder)
        {
            builder
                .HasOne(x => x.Content);
        }
    }
}