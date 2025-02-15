namespace SharmalRealEstateSystem.Api.Features.Admin.Car;

public class BL_Car
{
    #region Initializations

    private readonly IAdminUnitOfWork _adminUnitOfWork;

    public BL_Car(IAdminUnitOfWork adminUnitOfWork)
    {
        _adminUnitOfWork = adminUnitOfWork;
    }

    #endregion

    #region Get Car List

    public async Task<Result<CarListResponseModel>> GetCarList(GetCarListRequestModel requestModel)
    {
        Result<CarListResponseModel> responseModel;
        try
        {
            if (requestModel.PageNo <= 0)
            {
                responseModel = GetInvalidPageNoResult();
                goto result;
            }

            if (requestModel.PageSize <= 0)
            {
                responseModel = GetInvalidPageSizeResult();
                goto result;
            }

            responseModel = await _adminUnitOfWork.CarRepository.GetCarList(requestModel);
        }
        catch (Exception ex)
        {
            responseModel = Result<CarListResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Get Car By Car Id

    public async Task<Result<CarDataModel>> GetCarByCarId(string id)
    {
        Result<CarDataModel> responseModel;
        try
        {
            if (id.IsNullOrEmpty())
            {
                responseModel = Result<CarDataModel>.FailureResult(MessageResource.InvalidId);
                goto result;
            }

            responseModel = await _adminUnitOfWork.CarRepository.GetCarByCarId(id);
        }
        catch (Exception ex)
        {
            responseModel = Result<CarDataModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Get Car List By User Id

    public async Task<Result<CarListResponseModel>> GetCarListByUserId(string userId, int pageNo, int pageSize)
    {
        Result<CarListResponseModel> responseModel;
        try
        {
            if (userId.IsNullOrEmpty())
            {
                responseModel = Result<CarListResponseModel>.FailureResult(MessageResource.InvalidId);
                goto result;
            }

            if (pageNo <= 0)
            {
                responseModel = GetInvalidPageNoResult();
                goto result;
            }

            if (pageSize <= 0)
            {
                responseModel = GetInvalidPageSizeResult();
                goto result;
            }

            responseModel = await _adminUnitOfWork.CarRepository.GetCarListByUserId(userId, pageNo, pageSize);
        }
        catch (Exception ex)
        {
            responseModel = Result<CarListResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Create Car

    public async Task<Result<CarResponseModel>> CreateCar(CarRequestModel requestModel)
    {
        Result<CarResponseModel> responseModel;

        if (requestModel.Files is null || requestModel.Files.Count <= 0)
        {
            responseModel = GetInvalidFileResult();
            goto result;
        }

        responseModel = await _adminUnitOfWork.CarRepository.CreateCar(requestModel);

    result:
        return responseModel;
    }

    #endregion

    #region Update Car

    public async Task<Result<CarResponseModel>> UpdateCar(
        UpdateCarRequestModel requestModel,
        string carId
    )
    {
        Result<CarResponseModel> responseModel;

        if (carId.IsNullOrEmpty())
        {
            responseModel = GetInvalidIdResult();
            goto result;
        }

        responseModel = await _adminUnitOfWork.CarRepository.UpdateCar(requestModel, carId);

    result:
        return responseModel;
    }

    #endregion

    #region Delete Car

    public async Task<Result<CarResponseModel>> DeleteCar(string carId)
    {
        Result<CarResponseModel> responseModel;

        if (carId.IsNullOrEmpty())
        {
            responseModel = GetInvalidIdResult();
            goto result;
        }

        responseModel = await _adminUnitOfWork.CarRepository.DeleteCar(carId);

    result:
        return responseModel;
    }

    #endregion

    private Result<CarResponseModel> GetInvalidFileResult() =>
        Result<CarResponseModel>.FailureResult("Invalid Files.");

    private Result<CarResponseModel> GetInvalidIdResult() =>
        Result<CarResponseModel>.FailureResult(MessageResource.InvalidId);

    private Result<CarListResponseModel> GetInvalidPageNoResult() =>
        Result<CarListResponseModel>.FailureResult(MessageResource.InvalidPageNo);

    private Result<CarListResponseModel> GetInvalidPageSizeResult() =>
        Result<CarListResponseModel>.FailureResult(MessageResource.InvalidPageSize);
}
