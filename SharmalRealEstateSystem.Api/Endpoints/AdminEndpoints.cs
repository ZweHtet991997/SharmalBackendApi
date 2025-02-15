namespace SharmalRealEstateSystem.Api.Endpoints;

public class AdminEndpoints
{
    public static string UserList { get; } = "/api/v1/admin-account";
    public static string ResetFailCount { get; } = "/api/v1/account";
    public static string Register { get; } = "/api/v1/admin-account/register";
    public static string Login { get; } = "/api/v1/admin-account/login";
    public static string UpdateProfile { get; } = "/api/v1/admin-account/update-profile";
    public static string UpdatePassword { get; } = "/api/v1/account/update-password";
    public static string ExchangeRate { get; } = "/api/v1/admin-exchange-rate";
    public static string Property { get; } = "/api/v1/admin-property";
    public static string FilterProperty { get; } = "/api/v1/admin-property/filter";
    public static string Feature { get; } = "/api/v1/admin-feature";
    public static string Inquiry { get; } = "/api/v1/admin-inquiry";
    public static string FilterInquiry { get; } = "/api/v1/admin-inquiry/filter";
    public static string AdsPage { get; } = "/api/v1/admin-ads-page";
    public static string Ads { get; } = "/api/v1/admin-ads";
    public static string Car { get; } = "/api/v1/admin-car";
    public static string FilterCar { get; } = "/api/v1/admin-car/filter";

    private static string ApiPrefix { get; } = "/api/v1";

    public static List<string> GetAdminEndpoints()
    {
        return new List<string>
        {
            //$"{ApiPrefix}/account/login",
            //$"{ApiPrefix}/account/register",
            //$"{ApiPrefix}/admin-account",
            //$"{ApiPrefix}/account/update-password",
            //$"{ApiPrefix}/admin-account/update-profile",
            //$"{ApiPrefix}/ads",
            //$"{ApiPrefix}/admin-ads-page",
            //$"{ApiPrefix}/ads/filter",
            //$"{ApiPrefix}/property",
            //$"{ApiPrefix}/property/filter",
            //$"{ApiPrefix}/car",
            //$"{ApiPrefix}/car/filter",
            //$"{ApiPrefix}/exchange-rate",
            //$"{ApiPrefix}/feature",
            //$"{ApiPrefix}/inquiry",
            //$"{ApiPrefix}/inquiry/filter",
            //$"{ApiPrefix}/ads-page",
            //$"{ApiPrefix}/admin-feature",

            $"{ApiPrefix}/admin-account", // Get User List
            $"{ApiPrefix}/admin-account/register", // Create Account
            $"{ApiPrefix}/feature-account/login", // Login
            $"{ApiPrefix}/admin-account/update-profile", // Update Profile
            $"{ApiPrefix}/feature-account/update-password", // Update Password
            $"{ApiPrefix}/feature-ads",
            $"{ApiPrefix}/feature-ads/filter",
            $"{ApiPrefix}/admin-ads-page",
            $"{ApiPrefix}/feature-ads-page",
            $"{ApiPrefix}/feature-car",
            $"{ApiPrefix}/feature-car/filter",
            $"{ApiPrefix}/feature-car/car-id",
            $"{ApiPrefix}/feature-exchange-rate",
            $"{ApiPrefix}/admin-feature",
            $"{ApiPrefix}/feature-feature",
            $"{ApiPrefix}/feature-inquiry",
            $"{ApiPrefix}/feature-inquiry/filter",
            $"{ApiPrefix}/feature-property",
            $"{ApiPrefix}/feature-property/filter",
            $"{ApiPrefix}/feature-property/property-id",
            $"{ApiPrefix}/feature-dashboard"
        };
    }
}
