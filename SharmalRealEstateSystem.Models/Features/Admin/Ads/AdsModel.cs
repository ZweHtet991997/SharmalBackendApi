using System.Globalization;
using SharmalRealEstateSystem.Models.Enums.Ads;

namespace SharmalRealEstateSystem.Models.Features.Admin.Ads;

public class AdsModel
{
    public string AdsId { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string AdsCode { get; set; } = null!;

    public string TargetUrl { get; set; } = null!;

    public string? AdsLayout { get; set; }

    public string StartDate { get; set; } = null!;

    public string EndDate { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public string? UpdatedBy { get; set; }

    //[JsonProperty("Status")]
    //public bool IsDeleted { get; set; }

    public string? Status
    {
        get
        {
            if (
                DateTime.TryParseExact(
                    EndDate,
                    "M/d/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime parsedEndDate
                )
            )
            {
                return parsedEndDate > GetCurrentMyanmarDateTime()
                    ? Convert.ToString(EnumAdsStatus.Active)
                    : Convert.ToString(EnumAdsStatus.Expired);
            }
            else
            {
                return "Invalid date format";
            }
        }
    }

    public static DateTime GetCurrentMyanmarDateTime()
    {
        var myanmarTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Myanmar Standard Time");
        var myanmarDateTime = TimeZoneInfo.ConvertTime(
            DateTime.Now,
            TimeZoneInfo.Local,
            myanmarTimeZone
        );

        return myanmarDateTime;
    }
}
