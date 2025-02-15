namespace SharmalRealEstateSystem.Api.Endpoints;

public class StaffEndpoints
{
    public static string UpdateUser { get; } = "/api/v1/account";
    public static string UpdatePassword { get; } = "/api/v1/account/update-password";

    private static string ApiPrefix { get; } = "/api/v1";

    public static List<string> GetStaffEndpoints()
    {
        return new List<string>
        {
            //$"{ApiPrefix}/account",
            //$"{ApiPrefix}/account/register",
            //$"{ApiPrefix}/account/register",
            //$"{ApiPrefix}/account/update-password",
            //$"{ApiPrefix}/ads",
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

            $"{ApiPrefix}/feature-account/login", // Login
            $"{ApiPrefix}/feature-account/update-password", // Update Password
            $"{ApiPrefix}/feature-ads",
            $"{ApiPrefix}/feature-ads-page",
            $"{ApiPrefix}/feature-ads/filter",
            $"{ApiPrefix}/feature-car",
            $"{ApiPrefix}/feature-car/filter",
            $"{ApiPrefix}/feature-exchange-rate",
            $"{ApiPrefix}/feature-inquiry",
            $"{ApiPrefix}/feature-inquiry/filter",
            $"{ApiPrefix}/feature-property",
            $"{ApiPrefix}/feature-property/filter",
            $"{ApiPrefix}/feature-feature",
            $"{ApiPrefix}/feature-dashboard"
        };
    }
}
