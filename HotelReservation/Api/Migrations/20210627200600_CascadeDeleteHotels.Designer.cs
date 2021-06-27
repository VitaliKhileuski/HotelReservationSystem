﻿// <auto-generated />
using System;
using HotelReservation.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HotelReservation.Api.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20210627200600_CascadeDeleteHotels")]
    partial class CascadeDeleteHotels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HotelEntityUserEntity", b =>
                {
                    b.Property<Guid>("AdminsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OwnedHotelsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AdminsId", "OwnedHotelsId");

                    b.HasIndex("OwnedHotelsId");

                    b.ToTable("HotelEntityUserEntity");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.AttachmentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FileContentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileExtension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("HotelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FileContentId")
                        .IsUnique();

                    b.HasIndex("HotelId");

                    b.HasIndex("RoomId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.FileContentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AttachmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Content")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.HotelEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Hotels");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.LocationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BuildingNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("City")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<Guid>("HotelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("HotelId")
                        .IsUnique();

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.OrderEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOrdered")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("FullPrice")
                        .HasColumnType("decimal(18,4)");

                    b.Property<int>("NumberOfDays")
                        .HasColumnType("int");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.RefreshTokenEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.RoleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.RoomEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BedsNumber")
                        .HasColumnType("int");

                    b.Property<Guid>("HotelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("PaymentPerDay")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("RoomNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("HotelId");

                    b.HasIndex("UserId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.ServiceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HotelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<decimal>("Payment")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("Id");

                    b.HasIndex("HotelId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<Guid?>("RefreshTokenId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("RefreshTokenId")
                        .IsUnique()
                        .HasFilter("[RefreshTokenId] IS NOT NULL");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OrderEntityServiceEntity", b =>
                {
                    b.Property<Guid>("OrdersId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ServicesId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OrdersId", "ServicesId");

                    b.HasIndex("ServicesId");

                    b.ToTable("OrderEntityServiceEntity");
                });

            modelBuilder.Entity("HotelEntityUserEntity", b =>
                {
                    b.HasOne("HotelReservation.Data.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("AdminsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HotelReservation.Data.Entities.HotelEntity", null)
                        .WithMany()
                        .HasForeignKey("OwnedHotelsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.AttachmentEntity", b =>
                {
                    b.HasOne("HotelReservation.Data.Entities.FileContentEntity", "FileContent")
                        .WithOne("Attachment")
                        .HasForeignKey("HotelReservation.Data.Entities.AttachmentEntity", "FileContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HotelReservation.Data.Entities.HotelEntity", "Hotel")
                        .WithMany("Attachments")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HotelReservation.Data.Entities.RoomEntity", "Room")
                        .WithMany("Attachments")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FileContent");

                    b.Navigation("Hotel");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.LocationEntity", b =>
                {
                    b.HasOne("HotelReservation.Data.Entities.HotelEntity", "Hotel")
                        .WithOne("Location")
                        .HasForeignKey("HotelReservation.Data.Entities.LocationEntity", "HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.OrderEntity", b =>
                {
                    b.HasOne("HotelReservation.Data.Entities.RoomEntity", "Room")
                        .WithMany("Order")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HotelReservation.Data.Entities.UserEntity", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.RoomEntity", b =>
                {
                    b.HasOne("HotelReservation.Data.Entities.HotelEntity", "Hotel")
                        .WithMany("Rooms")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HotelReservation.Data.Entities.UserEntity", "User")
                        .WithMany("Rooms")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Hotel");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.ServiceEntity", b =>
                {
                    b.HasOne("HotelReservation.Data.Entities.HotelEntity", "Hotel")
                        .WithMany("Services")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.UserEntity", b =>
                {
                    b.HasOne("HotelReservation.Data.Entities.RefreshTokenEntity", "RefreshToken")
                        .WithOne("User")
                        .HasForeignKey("HotelReservation.Data.Entities.UserEntity", "RefreshTokenId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("HotelReservation.Data.Entities.RoleEntity", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RefreshToken");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("OrderEntityServiceEntity", b =>
                {
                    b.HasOne("HotelReservation.Data.Entities.OrderEntity", null)
                        .WithMany()
                        .HasForeignKey("OrdersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HotelReservation.Data.Entities.ServiceEntity", null)
                        .WithMany()
                        .HasForeignKey("ServicesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.FileContentEntity", b =>
                {
                    b.Navigation("Attachment");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.HotelEntity", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Location");

                    b.Navigation("Rooms");

                    b.Navigation("Services");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.RefreshTokenEntity", b =>
                {
                    b.Navigation("User");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.RoleEntity", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.RoomEntity", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("HotelReservation.Data.Entities.UserEntity", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Rooms");
                });
#pragma warning restore 612, 618
        }
    }
}
