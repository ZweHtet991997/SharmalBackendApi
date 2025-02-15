namespace SharmalRealEstateSystem.Api.Features.Admin.Middlewares;

public class CustomPropertyMiddleware
{
    private readonly PropertyValidator _propertyValidator;
    private readonly UpdatePropertyValidator _updatePropertyValidator;

    public CustomPropertyMiddleware(
        PropertyValidator propertyValidator,
        UpdatePropertyValidator updatePropertyValidator
    )
    {
        _propertyValidator = propertyValidator;
        _updatePropertyValidator = updatePropertyValidator;
    }

    #region Handle Property

    public async Task HandleProperty(RequestDelegate next, HttpContext context)
    {
        var formDataDictionary = await context.ReadFormDataAsync();
        var requestModel = GetPropertyRequestModel(formDataDictionary);

        ValidationResult validationResult = await _propertyValidator.ValidateAsync(requestModel);
        if (!validationResult.IsValid)
        {
            string errors = string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage));
            var responseModel = Result<PropertyResponseModel>.FailureResult(errors);

            await context.Response.WriteAsync(responseModel.SerializeObject());
            return;
        }

        context.Request.Body.Position = 0;
        await next(context);
    }

    #endregion

    #region Handle Update Property

    public async Task HandleUpdateProperty(RequestDelegate next, HttpContext context)
    {
        var formDataDictionary = await context.ReadFormDataAsync();
        var requestModel = GetUpdatePropertyRequestModel(formDataDictionary);

        ValidationResult validationResult = await _updatePropertyValidator.ValidateAsync(
            requestModel
        );
        if (!validationResult.IsValid)
        {
            string errors = string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage));
            var responseModel = Result<PropertyResponseModel>.FailureResult(errors);

            await context.Response.WriteAsync(responseModel.SerializeObject());
            return;
        }

        context.Request.Body.Position = 0;
        await next(context);
    }

    #endregion

    #region Get Update Property Request Model

    private UpdatePropertyRequestModel GetUpdatePropertyRequestModel(
        Dictionary<string, string> formDataDictionary
    )
    {
        return new UpdatePropertyRequestModel
        {
            Title = formDataDictionary.TryGetValue("Title", out var title) ? title : string.Empty,
            Status = formDataDictionary.TryGetValue("Status", out var status)
                ? status
                : string.Empty,
            Type = formDataDictionary.TryGetValue("Type", out var type) ? type : string.Empty,
            Price = formDataDictionary.TryGetValue("Price", out var price) ? price.ToInt32() : 0,
            PaymentOption = formDataDictionary.TryGetValue("PaymentOption", out var paymentOption)
                ? paymentOption
                : string.Empty,
            Location = formDataDictionary.TryGetValue("Location", out var location)
                ? location
                : string.Empty,
            City = formDataDictionary.TryGetValue("City", out var city) ? city : string.Empty,
            NumberOfViewers = formDataDictionary.TryGetValue(
                "NumberOfViewers",
                out var numberOfViewers
            )
                ? numberOfViewers
                : string.Empty,
            Bedrooms = formDataDictionary.TryGetValue("Bedrooms", out var bedrooms)
                ? bedrooms.ToInt32()
                : 0,
            Area = formDataDictionary.TryGetValue("Area", out var area) ? area : string.Empty,
            Condition = formDataDictionary.TryGetValue("Condition", out var condition)
                ? condition
                : string.Empty,
            Description = formDataDictionary.TryGetValue("Description", out var description)
                ? description
                : string.Empty,
            Furnished = formDataDictionary.TryGetValue("Furnished", out var furnished)
                ? furnished
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
            UpdatedBy = formDataDictionary.TryGetValue("UpdatedBy", out var updatedBy)
                ? updatedBy
                : string.Empty,
        };
    }

    #endregion

    #region Get Property Request Model

    private PropertyRequestModel GetPropertyRequestModel(
        Dictionary<string, string> formDataDictionary
    )
    {
        return new PropertyRequestModel
        {
            Title = formDataDictionary.TryGetValue("Title", out var title) ? title : string.Empty,
            Status = formDataDictionary.TryGetValue("Status", out var status)
                ? status
                : string.Empty,
            Type = formDataDictionary.TryGetValue("Type", out var type) ? type : string.Empty,
            Price = formDataDictionary.TryGetValue("Price", out var price) ? price.ToInt32() : 0,
            PaymentOption = formDataDictionary.TryGetValue("PaymentOption", out var paymentOption)
                ? paymentOption
                : string.Empty,
            Location = formDataDictionary.TryGetValue("Location", out var location)
                ? location
                : string.Empty,
            City = formDataDictionary.TryGetValue("City", out var city) ? city : string.Empty,
            NumberOfViewers = formDataDictionary.TryGetValue(
                "NumberOfViewers",
                out var numberOfViewers
            )
                ? numberOfViewers
                : string.Empty,
            Bedrooms = formDataDictionary.TryGetValue("Bedrooms", out var bedrooms)
                ? bedrooms.ToInt32()
                : 0,
            Area = formDataDictionary.TryGetValue("Area", out var area) ? area : string.Empty,
            Condition = formDataDictionary.TryGetValue("Condition", out var condition)
                ? condition
                : string.Empty,
            Floor = formDataDictionary.TryGetValue("Floor", out var floor) ? floor : string.Empty,
            Description = formDataDictionary.TryGetValue("Description", out var description)
                ? description
                : string.Empty,
            Furnished = formDataDictionary.TryGetValue("Furnished", out var furnished)
                ? furnished
                : string.Empty,
            MapUrl = formDataDictionary.TryGetValue("MapUrl", out var mapUrl)
                ? mapUrl
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
}
