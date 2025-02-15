namespace SharmalRealEstateSystem.Mapper.Features.Admin;

public static class ChangeModel
{
    #region User

    public static TblUser Map(this RegisterRequestModel requestModel)
    {
        return new TblUser
        {
            UserId = Ulid.NewUlid().ToString(),
            UserName = requestModel.UserName,
            Email = requestModel.Email,
            Password = requestModel.Password,
            UserRole = requestModel.UserRole,
            CreatedDate = DevCode.GetCurrentMyanmarDateTime(),
            FailedCount = 0,
            IsDeleted = false,
            CreatedBy = requestModel.CreatedBy,
            UpdatedBy = requestModel.UpdatedBy
        };
    }

    public static UserModel Map(this TblUser dataModel)
    {
        return new UserModel
        {
            UserId = dataModel.UserId,
            UserName = dataModel.UserName,
            Password = dataModel.Password,
            CreatedDate = dataModel.CreatedDate,
            Email = dataModel.Email,
            IsDeleted = dataModel.IsDeleted,
            UserRole = dataModel.UserRole,
            FailedCount = dataModel.FailedCount,
            CreatedBy = dataModel.CreatedBy,
            UpdatedBy = dataModel.UpdatedBy
        };
    }

    #endregion

    #region Exchange Rate

    public static ExchangeRateDataModel Map(this TblExchangeRate dataModel, string basePath)
    {
        return new ExchangeRateDataModel
        {
            ExchangeRateId = dataModel.ExchangeRateId,
            ExchangeRate = dataModel.ExchangeRate,
            Currency = dataModel?.Currency,
            UpdatedDate = dataModel?.UpdatedDate,
            Image = dataModel!.Currency!.ToString().ConvertFileToBase64(basePath)
        };
    }

    #endregion

    #region Property

    public static TblProperty Map(this PropertyRequestModel requestModel)
    {
        return new TblProperty
        {
            PropertyId = Ulid.NewUlid().ToString(),
            Title = requestModel.Title,
            Code = requestModel.Code,
            Status = requestModel.Status.ToString(),
            Type = requestModel.Type,
            Price = requestModel.Price,
            PaymentOption = requestModel.PaymentOption,
            Location = requestModel.Location,
            City = requestModel.City,
            NumberOfViewers = requestModel.NumberOfViewers,
            Bedrooms = requestModel.Bedrooms,
            Bathrooms = requestModel.Bathrooms,
            Area = requestModel.Area,
            Condition = requestModel.Condition,
            Floor = requestModel.Floor,
            Description = requestModel.Description,
            Furnished = requestModel.Furnished,
            MapUrl = requestModel.MapUrl,
            SellerName = requestModel.SellerName,
            PrimaryPhoneNumber = requestModel.PrimaryPhoneNumber,
            SecondaryPhoneNumber = requestModel.SecondaryPhoneNumber,
            Email = requestModel.Email,
            Address = requestModel.Address,
            CreatedBy = requestModel.CreatedBy,
            CreatedDate = DevCode.GetCurrentMyanmarDateTime(),
            IsDeleted = false,
            IsPopular = requestModel.IsPopular,
            Approved = (int)EnumPostStatus.Approved,
            IsHotDeal = requestModel.IsHotDeal
        };
    }

    public static PropertyModel Map(this TblProperty dataModel)
    {
        return new PropertyModel
        {
            Address = dataModel.Address,
            City = dataModel.City,
            Area = dataModel.Area,
            Condition = dataModel.Condition!,
            Bedrooms = dataModel.Bedrooms,
            Bathrooms = dataModel.Bedrooms,
            CreatedBy = dataModel.CreatedBy,
            CreatedDate = dataModel.CreatedDate,
            Description = dataModel.Description,
            Email = dataModel.Email,
            Floor = dataModel.Floor,
            Furnished = dataModel.Furnished!,
            IsDeleted = dataModel.IsDeleted,
            Location = dataModel.Location,
            MapUrl = dataModel.MapUrl,
            NumberOfViewers = dataModel.NumberOfViewers!,
            PaymentOption = dataModel.PaymentOption,
            Price = dataModel.Price,
            PrimaryPhoneNumber = dataModel.PrimaryPhoneNumber,
            PropertyId = dataModel.PropertyId,
            SecondaryPhoneNumber = dataModel.SecondaryPhoneNumber,
            SellerName = dataModel.SellerName,
            Status = dataModel.Status,
            Title = dataModel.Title,
            Code = dataModel.Code,
            Type = dataModel.Type,
            UpdatedBy = dataModel.UpdatedBy,
            UpdatedDate = dataModel.UpdatedDate,
            Approved = dataModel.Approved,
            IsHotDeal = dataModel.IsHotDeal,
            IsPopular = dataModel.IsPopular
        };
    }

    public static PropertyImageDataModel Map(this TblImage dataModel, string createdBy)
    {
        return new PropertyImageDataModel
        {
            ImageId = dataModel.ImageId,
            ImageName = dataModel.ImageName,
            PropertyId = dataModel.PropertyId,
            CreatedBy = createdBy
        };
    }

    #endregion

    #region Feature

    public static FeatureModel Map(this TblFeature dataModel)
    {
        return new FeatureModel
        {
            FeatureId = dataModel.FeatureId,
            Name = dataModel.Name,
            IsDeleted = dataModel.IsDeleted
        };
    }

    public static TblFeature Map(this FeatureRequestModel requestModel)
    {
        return new TblFeature
        {
            FeatureId = Ulid.NewUlid().ToString(),
            Name = requestModel.Name,
            IsDeleted = false
        };
    }

    #endregion

    #region Property Feature

    public static TblPropertyFeature Map(
        this PropertyFeatureRequestModel requestModel,
        string propertyId
    )
    {
        return new TblPropertyFeature
        {
            PropertyFeatureId = Ulid.NewUlid().ToString(),
            PropertyId = propertyId,
            FeatureId = requestModel.FeatureId!
        };
    }

    public static PropertyFeatureModel Map(this TblPropertyFeature dataModel)
    {
        return new PropertyFeatureModel
        {
            FeatureId = dataModel.FeatureId,
            PropertyFeatureId = dataModel.PropertyFeatureId,
            PropertyId = dataModel.PropertyId
        };
    }

    #endregion

    #region Inquiry

    public static InquiryModel Map(this TblInquire dataModel)
    {
        return new InquiryModel
        {
            CarId = dataModel.CarId,
            CreatedDate = dataModel.CreatedDate,
            Description = dataModel.Description,
            Email = dataModel.Email,
            InquiresId = dataModel.InquiresId,
            IsDeleted = dataModel.IsDeleted,
            PhoneNumber = dataModel.PhoneNumber,
            PropertyId = dataModel.PropertyId,
            IsDone = dataModel.IsDone,
            UpdatedDate = dataModel.UpdatedDate,
            UserName = dataModel.UserName,
        };
    }

    public static TblInquire Map(this InquiryRequestModel requestModel)
    {
        return new TblInquire
        {
            InquiresId = Ulid.NewUlid().ToString(),
            PropertyId = requestModel.PropertyId,
            CarId = requestModel.CarId,
            IsDone = false,
            CreatedDate = DevCode.GetCurrentMyanmarDateTime(),
            UserName = requestModel.UserName,
            PhoneNumber = requestModel.PhoneNumber,
            Email = requestModel.Email,
            Description = requestModel.Description,
            IsDeleted = false,
        };
    }

    public static InquiryDataModel Change(this TblInquire dataModel)
    {
        return new InquiryDataModel
        {
            CarId = dataModel.CarId,
            CreatedDate = dataModel.CreatedDate,
            Description = dataModel.Description,
            Email = dataModel.Email,
            InquiresId = dataModel.InquiresId,
            IsDeleted = dataModel.IsDeleted,
            IsDone = dataModel.IsDone,
            UpdatedDate = dataModel.UpdatedDate,
            UserName = dataModel.UserName,
            PhoneNumber = dataModel.PhoneNumber,
            PropertyId = dataModel.PropertyId,
        };
    }

    #endregion

    #region Ads Page

    public static AdsPageModel Map(this TblAdsPage dataModel)
    {
        return new AdsPageModel
        {
            AdsPageId = dataModel.AdsPageId,
            Pages = dataModel.Pages,
            IsDeleted = dataModel.IsDeleted
        };
    }

    public static TblAdsPage Map(this AdsPageRequestModel requestModel)
    {
        return new TblAdsPage
        {
            AdsPageId = Ulid.NewUlid().ToString(),
            Pages = requestModel.Pages,
            IsDeleted = false
        };
    }

    #endregion

    #region Ads

    public static TblAd Map(this AdsRequestModel requestModel)
    {
        return new TblAd
        {
            AdsId = Ulid.NewUlid().ToString(),
            CreatedBy = requestModel.CreatedBy,
            StartDate = requestModel.StartDate,
            EndDate = requestModel.EndDate,
            Title = requestModel.Title,
            TargetUrl = requestModel.TargetUrl,
            AdsLayout = requestModel.AdsLayout,
            IsDeleted = false
        };
    }

    public static AdsModel Map(this TblAd dataModel)
    {
        return new AdsModel
        {
            AdsId = dataModel.AdsId,
            CreatedBy = dataModel.CreatedBy,
            EndDate = dataModel.EndDate,
            //IsDeleted = dataModel.IsDeleted,
            StartDate = dataModel.StartDate,
            TargetUrl = dataModel.TargetUrl,
            Title = dataModel.Title,
            UpdatedBy = dataModel.UpdatedBy,
            AdsLayout = dataModel.AdsLayout
        };
    }

    #endregion

    #region Ads Page Placement

    public static AdsPagePlacementModel Map(this TblAdsPagePlacement dataModel)
    {
        return new AdsPagePlacementModel
        {
            AdsId = dataModel.AdsId,
            AdsPageId = dataModel.AdsPageId,
            AdsPagePlacementId = dataModel.AdsPagePlacementId
        };
    }

    #endregion

    #region Ads Page Placement

    public static TblAdsPagePlacement Map(
        this AdsPagePlacementRequestModel requestModel,
        string adsId
    )
    {
        return new TblAdsPagePlacement
        {
            AdsPagePlacementId = Ulid.NewUlid().ToString(),
            AdsId = adsId,
            AdsPageId = requestModel.AdsPageId
        };
    }

    #endregion

    #region Car

    public static TblCar Map(this CarRequestModel requestModel)
    {
        return new TblCar
        {
            Address = requestModel.Address,
            Approved = (int)EnumPostStatus.Approved,
            Availability = requestModel.Availability,
            CarColor = requestModel.CarColor,
            CarId = Ulid.NewUlid().ToString(),
            City = requestModel.City,
            Condition = requestModel.Condition,
            CreatedBy = requestModel.CreatedBy,
            CreatedDate = DevCode.GetCurrentMyanmarDateTime(),
            Status = requestModel.Status,
            Description = requestModel.Description,
            Email = requestModel.Email,
            EnginePower = requestModel.EnginePower,
            FuelType = requestModel.FuelType,
            Gearbox = requestModel.Gearbox,
            IsDeleted = false,
            IsHotDeal = requestModel.IsHotDeal,
            IsPopular = requestModel.IsPopular,
            IsSold = requestModel.IsSold,
            Location = requestModel.Location,
            Manufacturer = requestModel.Manufacturer,
            Mileage = requestModel.Mileage,
            Model = requestModel.Model,
            NumberOfViewers = requestModel.NumberOfViewers,
            PaymentOption = requestModel.PaymentOption,
            PlateColor = requestModel.PlateColor,
            PlateDivision = requestModel.PlateDivision,
            PlateNo = requestModel.PlateNo,
            LincenseStatus = requestModel.LincenseStatus,
            Price = requestModel.Price,
            PrimaryPhoneNumber = requestModel.PrimaryPhoneNumber,
            SecondaryPhoneNumber = requestModel.SecondaryPhoneNumber,
            SellerName = requestModel.SellerName,
            SpecialStatus = requestModel.SpecialStatus,
            SteeringPosition = requestModel.SteeringPosition,
            Title = requestModel.Title,
            Code = requestModel.Code,
            TrimName = requestModel.TrimName,
            Year = requestModel.Year,
            BuildType = requestModel.BuildType
        };
    }

    public static CarModel Map(this TblCar dataModel)
    {
        return new CarModel
        {
            Address = dataModel.Address,
            Approved = dataModel.Approved,
            Availability = dataModel.Availability,
            BuildType = dataModel.BuildType,
            CarColor = dataModel.CarColor,
            CarId = dataModel.CarId,
            City = dataModel.City,
            Condition = dataModel.Condition,
            CreatedBy = dataModel.CreatedBy,
            CreatedDate = dataModel.CreatedDate,
            Description = dataModel.Description,
            Email = dataModel.Email,
            EnginePower = dataModel.EnginePower,
            FuelType = dataModel.FuelType,
            Gearbox = dataModel.Gearbox,
            IsDeleted = dataModel.IsDeleted,
            IsHotDeal = dataModel.IsHotDeal,
            IsPopular = dataModel.IsPopular,
            IsSold = dataModel.IsSold,
            LincenseStatus = dataModel.LincenseStatus,
            Location = dataModel.Location,
            Manufacturer = dataModel.Manufacturer,
            Mileage = dataModel.Mileage,
            Model = dataModel.Model,
            NumberOfViewers = dataModel.NumberOfViewers,
            PaymentOption = dataModel.PaymentOption,
            PlateColor = dataModel.PlateColor,
            PlateDivision = dataModel.PlateDivision,
            PlateNo = dataModel.PlateNo,
            Price = dataModel.Price,
            PrimaryPhoneNumber = dataModel.PrimaryPhoneNumber,
            SecondaryPhoneNumber = dataModel.SecondaryPhoneNumber,
            SellerName = dataModel.SellerName,
            SpecialStatus = dataModel.SpecialStatus,
            SteeringPosition = dataModel.SteeringPosition,
            Title = dataModel.Title,
            Code = dataModel.Code,
            TrimName = dataModel.TrimName,
            UpdatedBy = dataModel.UpdatedBy,
            UpdatedDate = dataModel.UpdatedDate,
            Year = dataModel.Year,
        };
    }

    #endregion
}
