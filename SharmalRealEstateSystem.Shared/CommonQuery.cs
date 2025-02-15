namespace SharmalRealEstateSystem.Shared;

public class CommonQuery
{
    public static string GetFilterProperty { get; } = "GetFilterProperty";
    public static string GetFilterPropertyInquiry { get; } = "GetFilterPropertyInquiry";
    public static string PropertyInquiryCountResult { get; } = "PropertyInquiryCountResult";
    public static string GetFilterCar { get; } = "GetFilterCar";
    public static string CarCountResult { get; } = "CarCountResult";
    public static string GetFilterAds { get; } = "GetFilterAds";
    public static string PropertyCountResult { get; } = "PropertyCountResult";
    public static string AdsCountResult { get; } = "AdsCountResult";
    public static string Sp_InquiryPropertyForSaleCount { get; } = "Sp_InquiryPropertyForSaleCount";
    public static string Sp_InquiryPropertyForRentCount { get; } = "Sp_InquiryPropertyForRentCount";
    public static string Sp_InquiryCarCount { get; } = "Sp_InquiryCarCount";
    public static string GetPropertyFeatureListByPropertyId { get; } =
        @"SELECT PropertyFeatureId, PropertyId, Tbl_Feature.FeatureId, Tbl_Feature.Name 
AS FeatureName FROM Tbl_PropertyFeature
INNER JOIN Tbl_Feature ON Tbl_PropertyFeature.FeatureId = Tbl_Feature.FeatureId
WHERE PropertyId = @PropertyId;";
}
