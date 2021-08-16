using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using HotelReservation.Data.Configurations;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace HotelReservation.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {

        }

        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<HotelEntity> Hotels { get; set; }
        public DbSet<LocationEntity> Locations { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<ServiceEntity> Services { get; set; }
        public DbSet<FileContentEntity> Files { get; set; }
        public DbSet<AttachmentEntity> Attachments { get; set; }
        public DbSet<ServiceQuantityEntity> ServiceQuantities { get; set; }
        public DbSet<EmailVerificationEntity> EmailVerificationEntities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.ApplyConfiguration(new UserEntityConfiguration())
                .ApplyConfiguration(new HotelEntityConfiguration())
                .ApplyConfiguration(new LocationEntityConfiguration())
                .ApplyConfiguration(new OrderEntityConfiguration())
                .ApplyConfiguration(new RoomEntityConfiguration())
                .ApplyConfiguration(new RefreshTokenConfiguration())
                .ApplyConfiguration(new ServiceEntityConfiguration())
                .ApplyConfiguration(new RoleEntityConfiguration())
                .ApplyConfiguration(new AttachmentEntityConfiguration())
                .ApplyConfiguration(new FileContentConfiguration());
        }
    }
}