namespace SharmalRealEstateSystem.Repositories.Features.Admin.Car;

public interface ICarRepository
{
    Task<Result<CarListResponseModel>> GetCarList(GetCarListRequestModel requestModel);
    Task<Result<CarDataModel>> GetCarByCarId(string id);
    Task<Result<CarListResponseModel>> GetCarListByUserId(string userId, int pageNo, int pageSize);
    Task<Result<CarResponseModel>> CreateCar(CarRequestModel requestModel);
    Task<Result<CarResponseModel>> UpdateCar(UpdateCarRequestModel requestModel, string carId);
    Task<Result<CarResponseModel>> DeleteCar(string carId);
}
