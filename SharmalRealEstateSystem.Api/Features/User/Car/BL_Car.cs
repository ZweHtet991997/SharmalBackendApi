namespace SharmalRealEstateSystem.Api.Features.User.Car;

public class BL_Car
{
    private readonly IAdminUnitOfWork _adminUnitOfWork;

    public BL_Car(IAdminUnitOfWork adminUnitOfWork)
    {
        _adminUnitOfWork = adminUnitOfWork;
    }

    public async Task<Result<CarListResponseModel>> GetCarList(GetCarListRequestModel requestModel)
    {
        return await _adminUnitOfWork.CarRepository.GetCarList(requestModel);
    }
}
