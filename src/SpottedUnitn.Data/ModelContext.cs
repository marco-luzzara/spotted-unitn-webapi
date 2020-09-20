using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpottedUnitn.Model.ShopAggregate;
using SpottedUnitn.Model.ShopAggregate.ValueObjects;
using SpottedUnitn.Model.UserAggregate;
using SpottedUnitn.Model.UserAggregate.ValueObjects;
using System;

namespace SpottedUnitn.Data
{
    public class ModelContext : DbContext
    {
        public ModelContext(DbContextOptions<ModelContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Shop> Shops { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            OnUserModelCreating(modelBuilder);
            OnShopModelCreating(modelBuilder);
        }

        protected void OnUserModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .HasField("id");

            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .HasField("name")
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.LastName)
                .HasField("lastName")
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.SubscriptionDate)
                .HasField("subscriptionDate");

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasField("role")
                .IsRequired()
                .HasConversion(new EnumToNumberConverter<User.UserRole, int>());

            modelBuilder.Entity<User>()
                .OwnsOne(u => u.Credentials, ownBuilder =>
                {
                    ownBuilder.Property(c => c.HashedPwd)
                        .IsRequired()
                        .HasColumnType("VARCHAR(72)")
                        .HasMaxLength(72);

                    ownBuilder.Ignore(c => c.Password);

                    ownBuilder.Property(c => c.Mail)
                        .IsRequired()
                        .HasColumnType("NVARCHAR(320)")
                        .HasMaxLength(320);
                });

            modelBuilder.Entity<UserProfilePhoto>(builder =>
            {
                builder.HasKey(upp => upp.UserId);

                builder.Property(upp => upp.UserId)
                    .HasField("userId");

                builder.Property(upp => upp.ProfilePhoto)
                    .HasField("profilePhoto")
                    .IsRequired();
            });

            modelBuilder.Entity<User>()
                .HasOne<UserProfilePhoto>(u => u.ProfilePhoto)
                .WithOne()
                .HasForeignKey<UserProfilePhoto>(upp => upp.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<User>()
            //    .OwnsOne(u => u.ProfilePhoto, builder =>
            //    {
            //        builder.Metadata.IsOwnership = false;
            //        builder.Metadata.IsRequired = false;
            //        builder.Metadata.PrincipalToDependent.SetIsEagerLoaded(false);

            //        builder.ToTable("UserProfilePhoto");

            //        builder.Property(u => u.ProfilePhoto)
            //            .IsRequired();
            //    });
        }

        protected void OnShopModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shop>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Shop>()
                .Property(s => s.Id)
                .HasField("id");

            modelBuilder.Entity<Shop>()
                .Property(s => s.Name)
                .HasField("name")
                .IsRequired();

            modelBuilder.Entity<Shop>()
                .Property(s => s.Description)
                .HasField("description")
                .IsRequired();

            modelBuilder.Entity<Shop>()
                .Property(s => s.LinkToSite)
                .HasField("linkToSite")
                .HasDefaultValue("");

            modelBuilder.Entity<Shop>()
                .Property(s => s.Discount)
                .HasField("discount")
                .IsRequired();

            modelBuilder.Entity<Shop>()
                .Property(s => s.PhoneNumber)
                .HasField("phoneNumber")
                .HasColumnType("VARCHAR(15)")
                .HasMaxLength(15)
                .HasDefaultValue("");

            modelBuilder.Entity<Shop>()
                .OwnsOne(s => s.Location, ownBuilder =>
                {
                    ownBuilder.Property(c => c.Address)
                        .IsRequired();

                    ownBuilder.Property(c => c.City)
                        .IsRequired();

                    ownBuilder.Property(c => c.Province)
                        .IsRequired();

                    ownBuilder.Property(c => c.PostalCode)
                        .HasMaxLength(16)
                        .IsRequired();

                    ownBuilder.Property(c => c.Latitude)
                        .IsRequired();

                    ownBuilder.Property(c => c.Longitude)
                        .IsRequired();
                });

            modelBuilder.Entity<ShopCoverPicture>(builder =>
            {
                builder.HasKey(scp => scp.ShopId);

                builder.Property(scp => scp.ShopId)
                    .HasField("shopId");

                builder.Property(upp => upp.CoverPicture)
                    .HasField("coverPicture")
                    .IsRequired();
            });

            modelBuilder.Entity<Shop>()
                .HasOne<ShopCoverPicture>(s => s.CoverPicture)
                .WithOne()
                .HasForeignKey<ShopCoverPicture>(scp => scp.ShopId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Shop>()
            //    .OwnsOne(s => s.CoverPicture, builder =>
            //    {
            //        builder.Metadata.IsOwnership = false;
            //        builder.Metadata.IsRequired = false;
            //        builder.Metadata.PrincipalToDependent.SetIsEagerLoaded(false);

            //        builder.ToTable("ShopCoverPicture");

            //        builder.Property(s => s.CoverPicture)
            //            .HasDefaultValue(new byte[] { })
            //            .IsRequired();
            //    });
        }
    }
}
