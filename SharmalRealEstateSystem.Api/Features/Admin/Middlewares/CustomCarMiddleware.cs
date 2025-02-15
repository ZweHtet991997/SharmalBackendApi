namespace SharmalRealEstateSystem.Api.Features.Admin.Middlewares;

public class CustomCarMiddleware
{
    private readonly CarValidator _carValidator;
    private readonly UpdateCarValidator _updateCarValidator;

    public CustomCarMiddleware(CarValidator carValidator, UpdateCarValidator updateCarValidator)
    {
        _carValidator = carValidator;
        _updateCarValidator = updateCarValidator;
    }

    #region Handle Car

    public async Task HandleCar(RequestDelegate next, HttpContext context)
    {
        var formDataDictionary = await context.ReadFormDataAsync();
        var requestModel = GetCarRequestModel(formDataDictionary);

        ValidationResult validationResult = await _carValidator.ValidateAsync(requestModel);
        if (!validationResult.IsValid)
        {
            string errors = string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage));
            var responseModel = Result<CarResponseModel>.FailureResult(errors);

            await context.Response.WriteAsync(responseModel.SerializeObject());
            return;
        }

        await next(context);
    }

    #endregion

    #region Handle Update Car

    public async Task HandleUpdateCar(RequestDelegate next, HttpContext context)
    {
        var formDataDictionary = await context.ReadFormDataAsync();
        var requestModel = GetUpdateCarRequestModel(formDataDictionary);

        ValidationResult validationResult = await _updateCarValidator.ValidateAsync(requestModel);
        if (!validationResult.IsValid)
        {
            string errors = string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage));
            var responseModel = Result<CarResponseModel>.FailureResult(errors);

            await context.Response.WriteAsync(responseModel.SerializeObject());
            return;
        }

        await next(context);
    }

    #endregion

    #region Get Car Request Model

    private CarRequestModel GetCarRequestModel(Dictionary<string, string> formDataDictionary)
    {
        return new CarRequestModel
        {
            Title = formDataDictionary.TryGetValue("Title", out var title) ? title : string.Empty,
            Description = formDataDictionary.TryGetValue("Description", out var description)
                ? description
                : string.Empty,
            Gearbox = formDataDictionary.TryGetValue("Gearbox", out var gearBox)
                ? gearBox
                : string.Empty,
            SteeringPosition = formDataDictionary.TryGetValue(
                "SteeringPosition",
                out var steeringPosition
            )
                ? steeringPosition
                : string.Empty,
            EnginePower = formDataDictionary.TryGetValue("EnginePower", out var enginePower)
                ? enginePower
                : string.Empty,
            FuelType = formDataDictionary.TryGetValue("FuelType", out var fuelType)
                ? fuelType
                : string.Empty,
            Mileage = formDataDictionary.TryGetValue("Mileage", out var mileage)
                ? mileage
                : string.Empty,
            Manufacturer = formDataDictionary.TryGetValue("Manufacturer", out var manufacturer)
                ? manufacturer
                : string.Empty,
            BuildType = formDataDictionary.TryGetValue("BuildType", out var buildType)
                ? buildType
                : string.Empty,
            Model = formDataDictionary.TryGetValue("Model", out var model) ? model : string.Empty,
            Year = formDataDictionary.TryGetValue("Year", out var year) ? year : string.Empty,
            PlateDivision = formDataDictionary.TryGetValue("PlateDivision", out var plateDivision)
                ? plateDivision
                : string.Empty,
            PlateNo = formDataDictionary.TryGetValue("PlateNo", out var plateNo)
                ? plateNo
                : string.Empty,
            PlateColor = formDataDictionary.TryGetValue("PlateColor", out var plateColor)
                ? plateColor
                : string.Empty,
            LincenseStatus = formDataDictionary.TryGetValue(
                "LincenseStatus",
                out var lincenseStatus
            )
                ? lincenseStatus
                : string.Empty,
            CarColor = formDataDictionary.TryGetValue("CarColor", out var carColor)
                ? carColor
                : string.Empty,
            Condition = formDataDictionary.TryGetValue("Condition", out var condition)
                ? condition
                : string.Empty,
            Price = formDataDictionary.TryGetValue("Price", out var price) ? price : string.Empty,
            SpecialStatus = formDataDictionary.TryGetValue("SpecialStatus", out var specialStatus)
                ? specialStatus
                : string.Empty,
            NumberOfViewers = formDataDictionary.TryGetValue(
                "NumberOfViewers",
                out var numberOfViewers
            )
                ? numberOfViewers
                : string.Empty,
            Availability = formDataDictionary.TryGetValue("Availability", out var availability)
                ? availability
                : string.Empty,
            IsSold =
                formDataDictionary.TryGetValue("IsSold", out var isSoldString)
                && bool.TryParse(isSoldString, out bool isSoldValue)
                    ? (bool?)isSoldValue
                    : null,
            Location = formDataDictionary.TryGetValue("Location", out var location)
                ? location
                : string.Empty,
            City = formDataDictionary.TryGetValue("City", out var city) ? city : string.Empty,
            PaymentOption = formDataDictionary.TryGetValue("PaymentOption", out var paymentOption)
                ? paymentOption
                : string.Empty,
            TrimName = formDataDictionary.TryGetValue("TrimName", out var trimName)
                ? trimName
                : string.Empty,
            SellerName = formDataDictionary.TryGetValue("SellerName", out var sellerName)
                ? sellerName
                : string.Empty,
            PrimaryPhoneNumber = formDataDictionary.TryGetValue(
                "PrimaryPhoneNumber",
                out var primaryPhoneNumber
            )
                ? primaryPhoneNumber
                : string.Empty,
            SecondaryPhoneNumber = formDataDictionary.TryGetValue(
                "SecondaryPhoneNumber",
                out var secondaryPhoneNumber
            )
                ? secondaryPhoneNumber
                : string.Empty,
            Email = formDataDictionary.TryGetValue("Email", out var email) ? email : string.Empty,
            Address = formDataDictionary.TryGetValue("Address", out var address)
                ? address
                : string.Empty,
            CreatedBy = formDataDictionary.TryGetValue("CreatedBy", out var createdBy)
                ? createdBy
                : string.Empty,
            IsPopular =
                formDataDictionary.TryGetValue("IsPopular", out var isPopularString)
                && bool.TryParse(isPopularString, out bool isPopularValue)
                    ? (bool?)isPopularValue
                    : null,
            IsHotDeal =
                formDataDictionary.TryGetValue("IsHotDeal", out var isHotDealString)
                && bool.TryParse(isHotDealString, out bool isHotDealValue)
                    ? (bool?)isHotDealValue
                    : null
        };
    }

    #endregion

    #region Get Update Car Request Model

    private UpdateCarRequestModel GetUpdateCarRequestModel(
        Dictionary<string, string> formDataDictionary
    )
    {
        return new UpdateCarRequestModel
        {
            Title = formDataDictionary.TryGetValue("Title", out var title) ? title : string.Empty,
            Description = formDataDictionary.TryGetValue("Description", out var description)
                ? description
                : string.Empty,
            Gearbox = formDataDictionary.TryGetValue("Gearbox", out var gearBox)
                ? gearBox
                : string.Empty,
            SteeringPosition = formDataDictionary.TryGetValue(
                "SteeringPosition",
                out var steeringPosition
            )
                ? steeringPosition
                : string.Empty,
            EnginePower = formDataDictionary.TryGetValue("EnginePower", out var enginePower)
                ? enginePower
                : string.Empty,
            FuelType = formDataDictionary.TryGetValue("FuelType", out var fuelType)
                ? fuelType
                : string.Empty,
            Mileage = formDataDictionary.TryGetValue("Mileage", out var mileage)
                ? mileage
                : string.Empty,
            Manufacturer = formDataDictionary.TryGetValue("Manufacturer", out var manufacturer)
                ? manufacturer
                : string.Empty,
            BuildType = formDataDictionary.TryGetValue("BuildType", out var buildType)
                ? buildType
                : string.Empty,
            Model = formDataDictionary.TryGetValue("Model", out var model) ? model : string.Empty,
            Year = formDataDictionary.TryGetValue("Year", out var year) ? year : string.Empty,
            PlateDivision = formDataDictionary.TryGetValue("PlateDivision", out var plateDivision)
                ? plateDivision
                : string.Empty,
            PlateNo = formDataDictionary.TryGetValue("PlateNo", out var plateNo)
                ? plateNo
                : string.Empty,
            PlateColor = formDataDictionary.TryGetValue("PlateColor", out var plateColor)
                ? plateColor
                : string.Empty,
            LincenseStatus = formDataDictionary.TryGetValue(
                "LincenseStatus",
                out var lincenseStatus
            )
                ? lincenseStatus
                : string.Empty,
            CarColor = formDataDictionary.TryGetValue("CarColor", out var carColor)
                ? carColor
                : string.Empty,
            Condition = formDataDictionary.TryGetValue("Condition", out var condition)
                ? condition
                : string.Empty,
            Price = formDataDictionary.TryGetValue("Price", out var price) ? price : string.Empty,
            SpecialStatus = formDataDictionary.TryGetValue("SpecialStatus", out var specialStatus)
                ? specialStatus
                : string.Empty,
            NumberOfViewers = formDataDictionary.TryGetValue(
                "NumberOfViewers",
                out var numberOfViewers
            )
                ? numberOfViewers
                : string.Empty,
            Availability = formDataDictionary.TryGetValue("Availability", out var availability)
                ? availability
                : string.Empty,
            IsSold =
                formDataDictionary.TryGetValue("IsSold", out var isSoldString)
                && bool.TryParse(isSoldString, out bool isSoldValue)
                    ? (bool?)isSoldValue
                    : null,
            Location = formDataDictionary.TryGetValue("Location", out var location)
                ? location
                : string.Empty,
            City = formDataDictionary.TryGetValue("City", out var city) ? city : string.Empty,
            PaymentOption = formDataDictionary.TryGetValue("PaymentOption", out var paymentOption)
                ? paymentOption
                : string.Empty,
            TrimName = formDataDictionary.TryGetValue("TrimName", out var trimName)
                ? trimName
                : string.Empty,
            SellerName = formDataDictionary.TryGetValue("SellerName", out var sellerName)
                ? sellerName
                : string.Empty,
            PrimaryPhoneNumber = formDataDictionary.TryGetValue(
                "PrimaryPhoneNumber",
                out var primaryPhoneNumber
            )
                ? primaryPhoneNumber
                : string.Empty,
            SecondaryPhoneNumber = formDataDictionary.TryGetValue(
                "SecondaryPhoneNumber",
                out var secondaryPhoneNumber
            )
                ? secondaryPhoneNumber
                : string.Empty,
            Email = formDataDictionary.TryGetValue("Email", out var email) ? email : string.Empty,
            Address = formDataDictionary.TryGetValue("Address", out var address)
                ? address
                : string.Empty,
            UpdatedBy = formDataDictionary.TryGetValue("UpdatedBy", out var updatedBy)
                ? updatedBy
                : string.Empty,
            IsPopular =
                formDataDictionary.TryGetValue("IsPopular", out var isPopularString)
                && bool.TryParse(isPopularString, out bool isPopularValue)
                    ? (bool?)isPopularValue
                    : null,
            IsHotDeal =
                formDataDictionary.TryGetValue("IsHotDeal", out var isHotDealString)
                && bool.TryParse(isHotDealString, out bool isHotDealValue)
                    ? (bool?)isHotDealValue
                    : null
        };
    }

    #endregion
}
