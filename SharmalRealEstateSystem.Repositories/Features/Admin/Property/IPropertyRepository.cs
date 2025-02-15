namespace SharmalRealEstateSystem.Repositories.Features.Admin.Property;

public interface IPropertyRepository
{
    Task<Result<PropertyListResponseModel>> GetList(GetPropertyListRequestModel requestModel);
    Task<Result<PropertyDataModel>> GetPropertyByPropertyId(string id);
    Task<Result<PropertyListResponseModel>> GetListByUserId(
        string userId,
        int pageNo,
        int pageSize
    );
    Task<Result<PropertyResponseModel>> CreateProperty(PropertyRequestModel requestModel);
    Task<Result<PropertyResponseModel>> CreatePropertyV1(PropertyRequestModel requestModel);
    Task<Result<PropertyResponseModel>> UpdateProperty(
        UpdatePropertyRequestModel requestModel,
        string propertyId
    );
    Task<Result<PropertyResponseModel>> DeleteProperty(string propertyId);
    Task<Result<PropertyResponseModel>> DeletePropertyV1(string propertyId);
}
