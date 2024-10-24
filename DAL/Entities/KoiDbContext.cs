﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

public partial class KoiDbContext : DbContext
{
    public KoiDbContext()
    {
    }

    public KoiDbContext(DbContextOptions<KoiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<Breed> Breeds { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<Koi> Kois { get; set; }

    public virtual DbSet<KoiBreed> KoiBreeds { get; set; }

    public virtual DbSet<KoiFarm> KoiFarms { get; set; }

    public virtual DbSet<KoiImage> KoiImages { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderStorage> OrderStorages { get; set; }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<RefundRequest> RefundRequests { get; set; }

    public virtual DbSet<RefundRequestMedium> RefundRequestMedia { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<ShippingFee> ShippingFees { get; set; }

    public virtual DbSet<StorageProvince> StorageProvinces { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;uid=SA;pwd=12345;database=KoiDB;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(e => e.AirportId).HasName("PK__Airport__E3DBE0EA5985119C");

            entity.ToTable("Airport");

            entity.Property(e => e.AirportId).ValueGeneratedNever();
            entity.Property(e => e.AirportName).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
        });

        modelBuilder.Entity<Breed>(entity =>
        {
            entity.HasKey(e => e.BreedId).HasName("PK__Breed__D1E9AE9DFA3E928C");

            entity.ToTable("Breed");

            entity.Property(e => e.BreedId).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD7B7900690DE");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Koi).WithMany(p => p.Carts)
                .HasForeignKey(d => d.KoiId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__KoiId__5BE2A6F2");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__UserId__5CD6CB2B");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.FlightId).HasName("PK__Flight__8A9E14EEC0BF8826");

            entity.ToTable("Flight");

            entity.Property(e => e.FlightId).ValueGeneratedNever();
            entity.Property(e => e.Airline).HasMaxLength(50);
            entity.Property(e => e.ArrivalDate).HasColumnType("datetime");
            entity.Property(e => e.DepartureDate).HasColumnType("datetime");
            entity.Property(e => e.FlightCode).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.ArrivalAirport).WithMany(p => p.FlightArrivalAirports)
                .HasForeignKey(d => d.ArrivalAirportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Flight__ArrivalA__31EC6D26");

            entity.HasOne(d => d.DepartureAirport).WithMany(p => p.FlightDepartureAirports)
                .HasForeignKey(d => d.DepartureAirportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Flight__Departur__30F848ED");
        });

        modelBuilder.Entity<Koi>(entity =>
        {
            entity.HasKey(e => e.KoiId).HasName("PK__Koi__E03435984E719D9F");

            entity.ToTable("Koi");

            entity.Property(e => e.KoiId).ValueGeneratedNever();
            entity.Property(e => e.Dob).HasColumnType("datetime");
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Farm).WithMany(p => p.Kois)
                .HasForeignKey(d => d.FarmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Koi__FarmId__403A8C7D");

            entity.HasOne(d => d.Order).WithMany(p => p.Kois)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Koi__OrderId__412EB0B6");
        });

        modelBuilder.Entity<KoiBreed>(entity =>
        {
            entity.HasKey(e => e.KoiBreedId).HasName("PK__KoiBreed__9C7F085B8377DE70");

            entity.ToTable("KoiBreed");

            entity.Property(e => e.KoiBreedId).ValueGeneratedNever();

            entity.HasOne(d => d.Breed).WithMany(p => p.KoiBreeds)
                .HasForeignKey(d => d.BreedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KoiBreed__BreedI__440B1D61");

            entity.HasOne(d => d.Koi).WithMany(p => p.KoiBreeds)
                .HasForeignKey(d => d.KoiId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KoiBreed__KoiId__44FF419A");
        });

        modelBuilder.Entity<KoiFarm>(entity =>
        {
            entity.HasKey(e => e.KoiFarmId).HasName("PK__KoiFarm__AC58E69BF3BBDC79");

            entity.ToTable("KoiFarm");

            entity.HasIndex(e => e.KoiFarmManagerId, "UQ__KoiFarm__4C18D4CCBECD39D5").IsUnique();

            entity.Property(e => e.KoiFarmId).ValueGeneratedNever();
            entity.Property(e => e.FarmAddress).HasMaxLength(200);
            entity.Property(e => e.FarmName).HasMaxLength(100);

            entity.HasOne(d => d.KoiFarmManager).WithOne(p => p.KoiFarm)
                .HasForeignKey<KoiFarm>(d => d.KoiFarmManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KoiFarm__KoiFarm__36B12243");

            entity.HasOne(d => d.StorageProvince).WithMany(p => p.KoiFarms)
                .HasForeignKey(d => d.StorageProvinceId)
                .HasConstraintName("FK__KoiFarm__Storage__35BCFE0A");
        });

        modelBuilder.Entity<KoiImage>(entity =>
        {
            entity.HasKey(e => e.KoiImageId).HasName("PK__KoiImage__11060AD2D7329BAD");

            entity.ToTable("KoiImage");

            entity.Property(e => e.KoiImageId).ValueGeneratedNever();

            entity.HasOne(d => d.Koi).WithMany(p => p.KoiImages)
                .HasForeignKey(d => d.KoiId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KoiImage__KoiId__47DBAE45");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BCF2229CDD6");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.OrderNumber).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__CustomerI__3C69FB99");

            entity.HasOne(d => d.Flight).WithMany(p => p.Orders)
                .HasForeignKey(d => d.FlightId)
                .HasConstraintName("FK__Order__FlightId__3D5E1FD2");
        });

        modelBuilder.Entity<OrderStorage>(entity =>
        {
            entity.HasKey(e => e.OrderStorageId).HasName("PK__OrderSto__5DEFA466A60CCF8B");

            entity.ToTable("OrderStorage");

            entity.Property(e => e.OrderStorageId).ValueGeneratedNever();
            entity.Property(e => e.ArrivalTime).HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderStorages)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderStor__Order__4BAC3F29");

            entity.HasOne(d => d.Shipper).WithMany(p => p.OrderStorages)
                .HasForeignKey(d => d.ShipperId)
                .HasConstraintName("FK__OrderStor__Shipp__4CA06362");

            entity.HasOne(d => d.StorageProvince).WithMany(p => p.OrderStorages)
                .HasForeignKey(d => d.StorageProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderStor__Stora__4AB81AF0");
        });

        modelBuilder.Entity<Policy>(entity =>
        {
            entity.HasKey(e => e.PolicyId).HasName("PK__Policy__2E1339A4E6515A37");

            entity.ToTable("Policy");

            entity.Property(e => e.PolicyId).ValueGeneratedNever();
            entity.Property(e => e.PolicyName).HasMaxLength(100);

            entity.HasOne(d => d.Farm).WithMany(p => p.Policies)
                .HasForeignKey(d => d.FarmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Policy__FarmId__398D8EEE");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.RefreshTokenId).HasName("PK__RefreshT__F5845E395C53B82C");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.RefreshTokenId).ValueGeneratedNever();
            entity.Property(e => e.ExpiredAt).HasColumnType("datetime");
            entity.Property(e => e.RefreshToken1).HasColumnName("RefreshToken");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefreshTo__UserI__59063A47");
        });

        modelBuilder.Entity<RefundRequest>(entity =>
        {
            entity.HasKey(e => e.RefundRequestId).HasName("PK__RefundRe__A67BF2290C984E2B");

            entity.ToTable("RefundRequest");

            entity.Property(e => e.RefundRequestId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Order).WithMany(p => p.RefundRequests)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefundReq__Order__4F7CD00D");

            entity.HasOne(d => d.Policy).WithMany(p => p.RefundRequests)
                .HasForeignKey(d => d.PolicyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefundReq__Polic__5070F446");
        });

        modelBuilder.Entity<RefundRequestMedium>(entity =>
        {
            entity.HasKey(e => e.RefundRequestMediaId).HasName("PK__RefundRe__D856E8F89C172C2C");

            entity.Property(e => e.RefundRequestMediaId).ValueGeneratedNever();

            entity.HasOne(d => d.RefundRequest).WithMany(p => p.RefundRequestMedia)
                .HasForeignKey(d => d.RefundRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefundReq__Refun__534D60F1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1A3657F91B");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<ShippingFee>(entity =>
        {
            entity.HasKey(e => e.ShippingFeeId).HasName("PK__Shipping__5463E6E6CDCC1C90");

            entity.ToTable("ShippingFee");

            entity.Property(e => e.ShippingFeeId).ValueGeneratedNever();
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.StorageProvinceJp).WithMany(p => p.ShippingFeeStorageProvinceJps)
                .HasForeignKey(d => d.StorageProvinceJpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShippingF__Stora__5FB337D6");

            entity.HasOne(d => d.StorageProvinceVn).WithMany(p => p.ShippingFeeStorageProvinceVns)
                .HasForeignKey(d => d.StorageProvinceVnId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShippingF__Stora__60A75C0F");
        });

        modelBuilder.Entity<StorageProvince>(entity =>
        {
            entity.HasKey(e => e.StorageProvinceId).HasName("PK__StorageP__D5625B68D8608A1F");

            entity.ToTable("StorageProvince");

            entity.Property(e => e.StorageProvinceId).ValueGeneratedNever();
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.ProvinceName).HasMaxLength(100);
            entity.Property(e => e.StorageName).HasMaxLength(100);

            entity.HasOne(d => d.Airport).WithMany(p => p.StorageProvinces)
                .HasForeignKey(d => d.AirportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StoragePr__Airpo__286302EC");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6B81932FAF");

            entity.ToTable("Transaction");

            entity.Property(e => e.TransactionId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethodTransaction).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.TransactionInfo).HasMaxLength(100);
            entity.Property(e => e.TransactionNumber).HasMaxLength(100);

            entity.HasOne(d => d.Order).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Order__5629CD9C");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4CD438C657");

            entity.ToTable("User");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.OtpExpiredTime).HasColumnType("datetime");
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.UserName).HasMaxLength(100);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__RoleId__2B3F6F97");

            entity.HasOne(d => d.StorageProvince).WithMany(p => p.Users)
                .HasForeignKey(d => d.StorageProvinceId)
                .HasConstraintName("FK__User__StoragePro__2C3393D0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
