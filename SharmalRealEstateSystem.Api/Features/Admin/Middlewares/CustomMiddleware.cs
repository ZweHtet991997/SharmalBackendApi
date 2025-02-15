namespace SharmalRealEstateSystem.Api.Features.Admin.Middlewares;

public class CustomMiddleware
{
    private readonly RequestDelegate _next;
    private readonly CustomAuthMiddleware _customAuthMiddleware;
    private readonly CustomExchangeRateMiddleware _customExchangeRateMiddleware;
    private readonly CustomPropertyMiddleware _customPropertyMiddleware;
    private readonly CustomFeatureMiddleware _customFeatureMiddleware;
    private readonly CustomInquiryMiddleware _customInquiryMiddleware;
    private readonly CustomAdsPageMiddleware _customAdsPageMiddleware;
    private readonly CustomAdsMiddleware _customAdsMiddleware;
    private readonly CustomCarMiddleware _customCarMiddleware;
    private readonly CustomUpdateProfileMiddleware _customUpdateProfileMiddleware;

    public CustomMiddleware(
        RequestDelegate next,
        CustomAuthMiddleware customAuthMiddleware,
        CustomExchangeRateMiddleware customExchangeRateMiddleware,
        CustomPropertyMiddleware customPropertyMiddleware,
        CustomFeatureMiddleware customFeatureMiddleware,
        CustomInquiryMiddleware customInquiryMiddleware,
        CustomAdsPageMiddleware customAdsPageMiddleware,
        CustomAdsMiddleware customAdsMiddleware,
        CustomCarMiddleware customCarMiddleware,
        CustomUpdateProfileMiddleware customUpdateProfileMiddleware
    )
    {
        _next = next;
        _customAuthMiddleware = customAuthMiddleware;
        _customExchangeRateMiddleware = customExchangeRateMiddleware;
        _customPropertyMiddleware = customPropertyMiddleware;
        _customFeatureMiddleware = customFeatureMiddleware;
        _customInquiryMiddleware = customInquiryMiddleware;
        _customAdsPageMiddleware = customAdsPageMiddleware;
        _customAdsMiddleware = customAdsMiddleware;
        _customCarMiddleware = customCarMiddleware;
        _customUpdateProfileMiddleware = customUpdateProfileMiddleware;
    }

    #region InvokeAsync

    //public async Task InvokeAsync(HttpContext context) // validate role
    //{
    //    if (
    //        context.Request.Path == AdminEndpoints.Register
    //        && context.Request.Method == EnumHttpMethod.POST.ToString()
    //    )
    //    {
    //        await _customAuthMiddleware.HandleRegister(context, _next);
    //    }
    //    else if (
    //        context.Request.Path == AdminEndpoints.Login
    //        && context.Request.Method == EnumHttpMethod.POST.ToString()
    //    )
    //    {
    //        await _customAuthMiddleware.HandleLogin(context, _next);
    //    }
    //    //else if (
    //    //    context.Request.Path.ToString().Contains(AdminEndpoints.ExchangeRate)
    //    //    && context.Request.Method == EnumHttpMethod.PUT.ToString()
    //    //)
    //    //{
    //    //    await _customExchangeRateMiddleware.HandleExchangeRate(context, _next);
    //    //}
    //    else if (
    //        context.Request.Path == AdminEndpoints.Property
    //        && context.Request.Method == EnumHttpMethod.POST.ToString()
    //    )
    //    {
    //        await _customPropertyMiddleware.HandleProperty(_next, context);
    //    }
    //    else if (
    //        context.Request.Path.ToString().Contains(Convert.ToString(AdminEndpoints.Property))
    //        && context.Request.Method == EnumHttpMethod.PUT.ToString()
    //    )
    //    {
    //        await _customPropertyMiddleware.HandleUpdateProperty(_next, context);
    //    }
    //    else if (
    //        context.Request.Path.ToString().Contains(AdminEndpoints.Feature)
    //        && (
    //            context.Request.Method == EnumHttpMethod.POST.ToString()
    //            || context.Request.Method == EnumHttpMethod.PUT.ToString()
    //        )
    //    )
    //    {
    //        await _customFeatureMiddleware.HandleFeature(_next, context);
    //    }
    //    else if (
    //        context.Request.Path.ToString().Contains(AdminEndpoints.Inquiry)
    //        && context.Request.Method == EnumHttpMethod.PUT.ToString()
    //    )
    //    {
    //        await _customInquiryMiddleware.HandleInquiry(_next, context);
    //    }
    //    else if (
    //        context.Request.Path == AdminEndpoints.Inquiry
    //        && context.Request.Method == Convert.ToString(EnumHttpMethod.POST)
    //    )
    //    {
    //        await _customInquiryMiddleware.HandleInquiry(_next, context);
    //    }
    //    else if (
    //        context.Request.Path.ToString().Contains(AdminEndpoints.AdsPage.ToString())
    //        && (
    //            context.Request.Method == EnumHttpMethod.POST.ToString()
    //            || context.Request.Method == EnumHttpMethod.PUT.ToString()
    //        )
    //    )
    //    {
    //        await _customAdsPageMiddleware.HandleAdsPage(_next, context);
    //    }
    //    else if (
    //        context.Request.Path == AdminEndpoints.Ads.ToString()
    //        && context.Request.Method == EnumHttpMethod.POST.ToString()
    //    )
    //    {
    //        await _customAdsMiddleware.HandleAds(_next, context);
    //    }
    //    else if (
    //        context.Request.Path.ToString().Contains(AdminEndpoints.Ads.ToString())
    //        && context.Request.Method == EnumHttpMethod.PUT.ToString()
    //    )
    //    {
    //        await _customAdsMiddleware.HandleUpdateAds(_next, context);
    //    }
    //    else if (
    //        context.Request.Path == AdminEndpoints.Car.ToString()
    //        && context.Request.Method == EnumHttpMethod.POST.ToString()
    //    )
    //    {
    //        await _customCarMiddleware.HandleCar(_next, context);
    //    }
    //    else if (
    //        context.Request.Path.ToString().Contains(AdminEndpoints.Car.ToString())
    //        && context.Request.Method == EnumHttpMethod.PUT.ToString()
    //    )
    //    {
    //        await _customCarMiddleware.HandleUpdateCar(_next, context);
    //    }
    //    else
    //    {
    //        await _next(context);
    //    }
    //}

    #endregion

    #region InvokeAsync V1

    public async Task InvokeAsync(HttpContext context) // validate role
    {
        if (IsRegister(context))
        {
            await _customAuthMiddleware.HandleRegister(context, _next);
        }
        else if (IsLogin(context))
        {
            await _customAuthMiddleware.HandleLogin(context, _next);
        }
        else if (IsUpdateProfile(context))
        {
            await _customUpdateProfileMiddleware.HandleUpdateProfile(_next, context);
        }
        else if (IsCreateProperty(context))
        {
            await _customPropertyMiddleware.HandleProperty(_next, context);
        }
        else if (IsUpdateProperty(context))
        {
            await _customPropertyMiddleware.HandleUpdateProperty(_next, context);
        }
        else if (IsFeature(context))
        {
            await _customFeatureMiddleware.HandleFeature(_next, context);
        }
        else if (IsUpdateInquiry(context))
        {
            await _customInquiryMiddleware.HandleInquiry(_next, context);
        }
        else if (IsCreateInquiry(context))
        {
            await _customInquiryMiddleware.HandleInquiry(_next, context);
        }
        else if (IsAdsPage(context))
        {
            await _customAdsPageMiddleware.HandleAdsPage(_next, context);
        }
        else if (IsCreateAds(context))
        {
            await _customAdsMiddleware.HandleAds(_next, context);
        }
        else if (IsUpdateAds(context))
        {
            await _customAdsMiddleware.HandleUpdateAds(_next, context);
        }
        else if (IsCreateCar(context))
        {
            await _customCarMiddleware.HandleCar(_next, context);
        }
        else if (IsUpdateCar(context))
        {
            await _customCarMiddleware.HandleUpdateCar(_next, context);
        }
        else
        {
            await _next(context);
        }
    }

    #endregion

    #region Is Register

    private bool IsRegister(HttpContext context) =>
        context.Request.Path == AdminEndpoints.Register
        && context.Request.Method == EnumHttpMethod.POST.ToString();

    #endregion

    #region Is Login

    private bool IsLogin(HttpContext context) =>
        context.Request.Path == AdminEndpoints.Login
        && context.Request.Method == EnumHttpMethod.POST.ToString();

    #endregion

    #region Is Create Property

    private bool IsCreateProperty(HttpContext context) =>
        context.Request.Path == AdminEndpoints.Property
        && context.Request.Method == EnumHttpMethod.POST.ToString();

    #endregion

    #region Is Update Property

    private bool IsUpdateProperty(HttpContext context) =>
        context.Request.Path.ToString().Contains(Convert.ToString(AdminEndpoints.Property))
        && context.Request.Method == EnumHttpMethod.PUT.ToString();

    #endregion

    #region Is Feature

    private bool IsFeature(HttpContext context) =>
        context.Request.Path.ToString().Contains(AdminEndpoints.Feature)
        && (
            context.Request.Method == EnumHttpMethod.POST.ToString()
            || context.Request.Method == EnumHttpMethod.PUT.ToString()
        );

    #endregion

    #region Is Update Inquiry

    private bool IsUpdateInquiry(HttpContext context) =>
        context.Request.Path.ToString().Contains(AdminEndpoints.Inquiry)
        && context.Request.Method == EnumHttpMethod.PUT.ToString();

    #endregion

    #region Is Create Inquiry

    private bool IsCreateInquiry(HttpContext context) =>
        context.Request.Path == AdminEndpoints.Inquiry
        && context.Request.Method == Convert.ToString(EnumHttpMethod.POST);

    #endregion

    #region Is Ads Page

    private bool IsAdsPage(HttpContext context) =>
        context.Request.Path.ToString().Contains(AdminEndpoints.AdsPage.ToString())
        && (
            context.Request.Method == EnumHttpMethod.POST.ToString()
            || context.Request.Method == EnumHttpMethod.PUT.ToString()
        );

    #endregion

    #region Is Create Ads

    private bool IsCreateAds(HttpContext context) =>
        context.Request.Path == AdminEndpoints.Ads.ToString()
        && context.Request.Method == EnumHttpMethod.POST.ToString();

    #endregion

    #region Is Update Ads

    private bool IsUpdateAds(HttpContext context) =>
        context.Request.Path.ToString().Contains(AdminEndpoints.Ads.ToString())
        && context.Request.Method == EnumHttpMethod.PUT.ToString();

    #endregion

    #region Is Create Car

    private bool IsCreateCar(HttpContext context) =>
        context.Request.Path == AdminEndpoints.Car.ToString()
        && context.Request.Method == EnumHttpMethod.POST.ToString();

    #endregion

    #region Is Update Car

    private bool IsUpdateCar(HttpContext context) =>
        context.Request.Path.ToString().Contains(AdminEndpoints.Car.ToString())
        && context.Request.Method == EnumHttpMethod.PUT.ToString();

    #endregion

    private bool IsUpdateProfile(HttpContext context) =>
        context.Request.Path == AdminEndpoints.UpdateProfile.ToString()
        && context.Request.Method == EnumHttpMethod.PUT.ToString();
}
