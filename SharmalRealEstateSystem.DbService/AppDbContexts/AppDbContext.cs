using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SharmalRealEstateSystem.DbService.AppDbContexts;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAd> TblAds { get; set; }

    public virtual DbSet<TblAdsPage> TblAdsPages { get; set; }

    public virtual DbSet<TblAdsPagePlacement> TblAdsPagePlacements { get; set; }

    public virtual DbSet<TblCar> TblCars { get; set; }

    public virtual DbSet<TblExchangeRate> TblExchangeRates { get; set; }

    public virtual DbSet<TblFeature> TblFeatures { get; set; }

    public virtual DbSet<TblImage> TblImages { get; set; }

    public virtual DbSet<TblInquire> TblInquires { get; set; }

    public virtual DbSet<TblMemberPlan> TblMemberPlans { get; set; }

    public virtual DbSet<TblProperty> TblProperties { get; set; }

    public virtual DbSet<TblPropertyFeature> TblPropertyFeatures { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TempTable> TempTables { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAd>(entity =>
        {
            entity.HasKey(e => e.AdsId);

            entity.ToTable("Tbl_Ads");

            entity.Property(e => e.AdsId).HasMaxLength(50);
            entity.Property(e => e.AdsLayout).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.EndDate).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasMaxLength(50);
            entity.Property(e => e.TargetUrl).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.UpdatedBy).HasMaxLength(250);
        });

        modelBuilder.Entity<TblAdsPage>(entity =>
        {
            entity.HasKey(e => e.AdsPageId);

            entity.ToTable("Tbl_AdsPage");

            entity.Property(e => e.AdsPageId).HasMaxLength(50);
            entity.Property(e => e.Pages).HasMaxLength(50);
        });

        modelBuilder.Entity<TblAdsPagePlacement>(entity =>
        {
            entity.HasKey(e => e.AdsPagePlacementId);

            entity.ToTable("Tbl_AdsPagePlacement");

            entity.Property(e => e.AdsPagePlacementId).HasMaxLength(50);
            entity.Property(e => e.AdsId).HasMaxLength(50);
            entity.Property(e => e.AdsPageId).HasMaxLength(50);
        });

        modelBuilder.Entity<TblCar>(entity =>
        {
            entity.HasKey(e => e.CarId);

            entity.ToTable("Tbl_Car");

            entity.Property(e => e.CarId).HasMaxLength(50);
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.Availability).HasMaxLength(50);
            entity.Property(e => e.BuildType).HasMaxLength(50);
            entity.Property(e => e.CarColor).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Condition).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDate).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.EnginePower).HasMaxLength(50);
            entity.Property(e => e.FuelType).HasMaxLength(50);
            entity.Property(e => e.Gearbox).HasMaxLength(50);
            entity.Property(e => e.IsHotDeal).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsPopular).HasDefaultValueSql("((0))");
            entity.Property(e => e.LincenseStatus).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.Manufacturer).HasMaxLength(50);
            entity.Property(e => e.Mileage).HasMaxLength(50);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.NumberOfViewers).HasMaxLength(50);
            entity.Property(e => e.PaymentOption).HasMaxLength(50);
            entity.Property(e => e.PlateColor).HasMaxLength(50);
            entity.Property(e => e.PlateDivision).HasMaxLength(50);
            entity.Property(e => e.PlateNo).HasMaxLength(50);
            entity.Property(e => e.Price).HasMaxLength(50);
            entity.Property(e => e.PrimaryPhoneNumber).HasMaxLength(50);
            entity.Property(e => e.SecondaryPhoneNumber).HasMaxLength(50);
            entity.Property(e => e.SellerName).HasMaxLength(50);
            entity.Property(e => e.SpecialStatus).HasMaxLength(50);
            entity.Property(e => e.SteeringPosition).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.TrimName).HasMaxLength(50);
            entity.Property(e => e.UpdatedBy).HasMaxLength(250);
            entity.Property(e => e.UpdatedDate).HasMaxLength(50);
            entity.Property(e => e.Year).HasMaxLength(50);
        });

        modelBuilder.Entity<TblExchangeRate>(entity =>
        {
            entity.HasKey(e => e.ExchangeRateId);

            entity.ToTable("Tbl_ExchangeRate");

            entity.Property(e => e.Currency).HasMaxLength(10);
            entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedDate).HasMaxLength(50);
        });

        modelBuilder.Entity<TblFeature>(entity =>
        {
            entity.HasKey(e => e.FeatureId);

            entity.ToTable("Tbl_Feature");

            entity.Property(e => e.FeatureId).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TblImage>(entity =>
        {
            entity.HasKey(e => e.ImageId);

            entity.ToTable("Tbl_Image");

            entity.Property(e => e.ImageId).HasMaxLength(50);
            entity.Property(e => e.AdsId).HasMaxLength(50);
            entity.Property(e => e.CarId).HasMaxLength(50);
            entity.Property(e => e.ImageName).HasMaxLength(250);
            entity.Property(e => e.PropertyId).HasMaxLength(50);
        });

        modelBuilder.Entity<TblInquire>(entity =>
        {
            entity.HasKey(e => e.InquiresId);

            entity.ToTable("Tbl_Inquires");

            entity.Property(e => e.InquiresId).HasMaxLength(50);
            entity.Property(e => e.CarId).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.PropertyId).HasMaxLength(50);
            entity.Property(e => e.UpdatedDate).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblMemberPlan>(entity =>
        {
            entity.HasKey(e => e.MemberPlanId);

            entity.ToTable("Tbl_MemberPlan");

            entity.Property(e => e.MemberPlanId).HasColumnName("MemberPlan_Id");
            entity.Property(e => e.ActiveDate).HasMaxLength(50);
            entity.Property(e => e.ExpireDate).HasMaxLength(50);
            entity.Property(e => e.MemberPlan).HasMaxLength(50);
        });

        modelBuilder.Entity<TblProperty>(entity =>
        {
            entity.HasKey(e => e.PropertyId);

            entity.ToTable("Tbl_Property");

            entity.Property(e => e.PropertyId).HasMaxLength(50);
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.Area).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Condition).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDate).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Floor).HasMaxLength(50);
            entity.Property(e => e.Furnished).HasMaxLength(50);
            entity.Property(e => e.IsHotDeal).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsPopular).HasDefaultValueSql("((0))");
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.MapUrl).HasMaxLength(250);
            entity.Property(e => e.NumberOfViewers).HasMaxLength(50);
            entity.Property(e => e.PaymentOption).HasMaxLength(50);
            entity.Property(e => e.PrimaryPhoneNumber).HasMaxLength(50);
            entity.Property(e => e.SecondaryPhoneNumber).HasMaxLength(50);
            entity.Property(e => e.SellerName).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.Code).HasMaxLength(250);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UpdatedBy).HasMaxLength(250);
            entity.Property(e => e.UpdatedDate).HasMaxLength(50);
        });

        modelBuilder.Entity<TblPropertyFeature>(entity =>
        {
            entity.HasKey(e => e.PropertyFeatureId);

            entity.ToTable("Tbl_PropertyFeature");

            entity.Property(e => e.PropertyFeatureId).HasMaxLength(50);
            entity.Property(e => e.FeatureId).HasMaxLength(50);
            entity.Property(e => e.PropertyId).HasMaxLength(50);

            entity.HasOne(d => d.Feature).WithMany(p => p.TblPropertyFeatures)
                .HasForeignKey(d => d.FeatureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_PropertyFeature_Tbl_Feature");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("Tbl_User");

            entity.Property(e => e.UserId).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
            entity.Property(e => e.UserRole).HasMaxLength(15);
        });

        modelBuilder.Entity<TempTable>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TempTable");

            entity.Property(e => e.CarId).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.InquiresId).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.PropertyId).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdatedDate).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
