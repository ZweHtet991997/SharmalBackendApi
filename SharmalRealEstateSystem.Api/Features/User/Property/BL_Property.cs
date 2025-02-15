namespace SharmalRealEstateSystem.Api.Features.User.Property;

public class BL_Property
{
    private readonly IAdminUnitOfWork _adminUnitOfWork;

    public BL_Property(IAdminUnitOfWork adminUnitOfWork)
    {
        _adminUnitOfWork = adminUnitOfWork;
    }

    public async Task<Result<PropertyListResponseModel>> GetList(
        GetPropertyListRequestModel requestModel
    )
    {
        return await _adminUnitOfWork.PropertyRepository.GetList(requestModel);
    }
}
