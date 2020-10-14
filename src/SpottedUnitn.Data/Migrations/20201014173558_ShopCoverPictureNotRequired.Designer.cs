﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpottedUnitn.Data;

namespace SpottedUnitn.Data.Migrations
{
    [DbContext(typeof(ModelContext))]
    [Migration("20201014173558_ShopCoverPictureNotRequired")]
    partial class ShopCoverPictureNotRequired
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SpottedUnitn.Model.ShopAggregate.Shop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LinkToSite")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("VARCHAR(15)")
                        .HasMaxLength(15)
                        .HasDefaultValue("");

                    b.HasKey("Id");

                    b.ToTable("Shops");
                });

            modelBuilder.Entity("SpottedUnitn.Model.ShopAggregate.ValueObjects.ShopCoverPicture", b =>
                {
                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.Property<byte[]>("CoverPicture")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("ShopId");

                    b.ToTable("ShopCoverPicture");
                });

            modelBuilder.Entity("SpottedUnitn.Model.UserAggregate.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("SubscriptionDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SpottedUnitn.Model.UserAggregate.ValueObjects.UserProfilePhoto", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<byte[]>("ProfilePhoto")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserProfilePhoto");
                });

            modelBuilder.Entity("SpottedUnitn.Model.ShopAggregate.Shop", b =>
                {
                    b.OwnsOne("SpottedUnitn.Model.ShopAggregate.ValueObjects.Location", "Location", b1 =>
                        {
                            b1.Property<int>("ShopId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Address")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<float>("Latitude")
                                .HasColumnType("real");

                            b1.Property<float>("Longitude")
                                .HasColumnType("real");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasColumnType("nvarchar(16)")
                                .HasMaxLength(16);

                            b1.Property<string>("Province")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ShopId");

                            b1.ToTable("Shops");

                            b1.WithOwner()
                                .HasForeignKey("ShopId");
                        });
                });

            modelBuilder.Entity("SpottedUnitn.Model.ShopAggregate.ValueObjects.ShopCoverPicture", b =>
                {
                    b.HasOne("SpottedUnitn.Model.ShopAggregate.Shop", null)
                        .WithOne("CoverPicture")
                        .HasForeignKey("SpottedUnitn.Model.ShopAggregate.ValueObjects.ShopCoverPicture", "ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SpottedUnitn.Model.UserAggregate.User", b =>
                {
                    b.OwnsOne("SpottedUnitn.Model.UserAggregate.ValueObjects.Credentials", "Credentials", b1 =>
                        {
                            b1.Property<int>("UserId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("HashedPwd")
                                .IsRequired()
                                .HasColumnType("VARCHAR(72)")
                                .HasMaxLength(72);

                            b1.Property<string>("Mail")
                                .IsRequired()
                                .HasColumnType("NVARCHAR(320)")
                                .HasMaxLength(320);

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });
                });

            modelBuilder.Entity("SpottedUnitn.Model.UserAggregate.ValueObjects.UserProfilePhoto", b =>
                {
                    b.HasOne("SpottedUnitn.Model.UserAggregate.User", null)
                        .WithOne("ProfilePhoto")
                        .HasForeignKey("SpottedUnitn.Model.UserAggregate.ValueObjects.UserProfilePhoto", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
