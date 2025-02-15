using System.Linq;

namespace SharmalRealEstateSystem.Repositories.Features.Admin.Car;

public class CarRepository : ICarRepository
{
    private readonly AppDbContext _context;
    private readonly FtpService _ftpService;
    private readonly DapperService _dapperService;
    private readonly AesService _aesService;

    public CarRepository(
        AppDbContext context,
        FtpService ftpService,
        DapperService dapperService,
        AesService aesService
    )
    {
        _context = context;
        _ftpService = ftpService;
        _dapperService = dapperService;
        _aesService = aesService;
    }

    #region Get Car List

    public async Task<Result<CarListResponseModel>> GetCarList(GetCarListRequestModel requestModel)
    {
        Result<CarListResponseModel> responseModel;
        try
        {
            var parameters = new
            {
                requestModel.PageNo,
                requestModel.PageSize,
                Manufacturer = requestModel.Manufacturer!.IsNullOrEmpty() ? null : requestModel.Manufacturer,
                Model = requestModel.Model!.IsNullOrEmpty() ? null : requestModel.Model,
                EnginePower = requestModel.EnginePower!.IsNullOrEmpty() ? null : requestModel.EnginePower,
                Gearbox = requestModel.Gearbox!.IsNullOrEmpty() ? null : requestModel.Gearbox,
                SteerPosition = requestModel.SteerPosition!.IsNullOrEmpty() ? null : requestModel.SteerPosition,
                Color = requestModel.Color!.IsNullOrEmpty() ? null : requestModel.Color,
                Code = requestModel.Code!.IsNullOrEmpty() ? null : requestModel.Code,
                Condition = requestModel.Condition!.IsNullOrEmpty() ? null : requestModel.Condition,
                MinYear = requestModel.MinYear!.IsNullOrEmpty() ? null : requestModel.MinYear,
                MaxYear = requestModel.MaxYear!.IsNullOrEmpty() ? null : requestModel.MaxYear,
                FuelType = requestModel.FuelType!.IsNullOrEmpty() ? null : requestModel.FuelType,
                IsPopular = requestModel.IsPopular!.IsNullOrEmpty() ? null : requestModel.IsPopular,
                IsHotDeal = requestModel.IsHotDeal!.IsNullOrEmpty() ? null : requestModel.IsHotDeal,
                MinPrice = requestModel.MinPrice is null || requestModel.MinPrice <= 0 ? null : requestModel.MinPrice,
                MaxPrice = requestModel.MaxPrice is null || requestModel.MaxPrice <= 0 ? null : requestModel.MaxPrice,
                MinMileAge = requestModel.MinMileAge is null || requestModel.MinMileAge <= 0 ? null : requestModel.MinMileAge,
                MaxMileAge = requestModel.MaxMileAge is null || requestModel.MaxMileAge <= 0 ? null : requestModel.MaxMileAge,
                BuildType = requestModel.BuildType!.IsNullOrEmpty() ? null : requestModel.BuildType
            };
            List<CarModel> lst = await _dapperService.QueryAsync<CarModel>(
                CommonQuery.GetFilterCar,
                parameters,
                CommandType.StoredProcedure
            );

            var totalCount = await _dapperService.GetTotalCountAsync(
                CommonQuery.CarCountResult,
                new
                {
                    Manufacturer = requestModel.Manufacturer!.IsNullOrEmpty() ? null : requestModel.Manufacturer,
                    Model = requestModel.Model!.IsNullOrEmpty() ? null : requestModel.Model,
                    EnginePower = requestModel.EnginePower!.IsNullOrEmpty() ? null : requestModel.EnginePower,
                    Gearbox = requestModel.Gearbox!.IsNullOrEmpty() ? null : requestModel.Gearbox,
                    SteerPosition = requestModel.SteerPosition!.IsNullOrEmpty() ? null : requestModel.SteerPosition,
                    Color = requestModel.Color!.IsNullOrEmpty() ? null : requestModel.Color,
                    Code = requestModel.Code!.IsNullOrEmpty() ? null : requestModel.Code,
                    Condition = requestModel.Condition!.IsNullOrEmpty() ? null : requestModel.Condition,
                    MinYear = requestModel.MinYear!.IsNullOrEmpty() ? null : requestModel.MinYear,
                    MaxYear = requestModel.MaxYear!.IsNullOrEmpty() ? null : requestModel.MaxYear,
                    FuelType = requestModel.FuelType!.IsNullOrEmpty() ? null : requestModel.FuelType,
                    IsPopular = requestModel.IsPopular!.IsNullOrEmpty() ? null : requestModel.IsPopular,
                    IsHotDeal = requestModel.IsHotDeal!.IsNullOrEmpty() ? null : requestModel.IsHotDeal,
                    MinPrice = requestModel.MinPrice is null || requestModel.MinPrice <= 0 ? null : requestModel.MinPrice,
                    MaxPrice = requestModel.MaxPrice is null || requestModel.MaxPrice <= 0 ? null : requestModel.MaxPrice,
                    MinMileAge = requestModel.MinMileAge is null || requestModel.MinMileAge <= 0 ? null : requestModel.MinMileAge,
                    MaxMileAge = requestModel.MaxMileAge is null || requestModel.MaxMileAge <= 0 ? null : requestModel.MaxMileAge,
                    BuildType = requestModel.BuildType!.IsNullOrEmpty() ? null : requestModel.BuildType
                },
                CommandType.StoredProcedure
            );

            var pageCount = totalCount / requestModel.PageSize;
            if (totalCount % requestModel.PageSize > 0)
            {
                pageCount++;
            }

            List<CarDataModel> dataLst = new();

            foreach (var item in lst)
            {
                var imgLstByCar = await _context
                    .TblImages.Where(x => x.CarId == item.CarId)
                    .ToListAsync();
                CarDataModel carDataModel =
                    new(
                        item,
                        imgLstByCar
                            .Select(x => new CarImageModel(x.ImageId, x.CarId!, x.ImageName, item.CreatedBy))
                            .ToList()
                    );

                dataLst.Add(carDataModel);
            }

            var pageSettingModel = new PageSettingModel(
                requestModel.PageNo,
                requestModel.PageSize,
                pageCount,
                totalCount
            );
            responseModel = Result<CarListResponseModel>.SuccessResult(
                new CarListResponseModel(dataLst, pageSettingModel)
            );
        }
        catch (Exception ex)
        {
            responseModel = Result<CarListResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion


    public async Task<Result<CarDataModel>> GetCarByCarId(string id)
    {
        Result<CarDataModel> responseModel;
        try
        {
            var car = await _context.TblCars.FindAsync(id);
            if (car is null)
            {
                responseModel = Result<CarDataModel>.FailureResult(MessageResource.NotFound, EnumStatusCode.NotFound);
                goto result;
            }

            var imgLstByCar = await _context.TblImages.Where(x => x.CarId == car.CarId).ToListAsync();
            var model = new CarDataModel(car.Map(), imgLstByCar.Select(x => new CarImageModel(x.ImageId, x.CarId!, x.ImageName, car.CreatedBy)).ToList());

            responseModel = Result<CarDataModel>.SuccessResult(model);
        }
        catch (Exception ex)
        {
            responseModel = Result<CarDataModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    public async Task<Result<CarListResponseModel>> GetCarListByUserId(string userId, int pageNo, int pageSize)
    {
        Result<CarListResponseModel> responseModel;
        try
        {
            var user = await _context.TblUsers.FindAsync(userId);
            if (user is null)
            {
                responseModel = Result<CarListResponseModel>.FailureResult("User Not Found.", EnumStatusCode.NotFound);
                goto result;
            }

            var query = _context.TblCars.OrderByDescending(x => x.CarId)
                .Where(x => x.CreatedBy == userId || x.UpdatedBy == userId && !x.IsDeleted);
            var lst = await query.Pagination(pageNo, pageSize).ToListAsync();
            var totalCount = await query.CountAsync();
            var pageCount = totalCount / pageSize;
            if (totalCount % pageSize > 0)
            {
                pageCount++;
            }

            var dataLst = new List<CarDataModel>();
            foreach (var item in lst)
            {
                var imageLst = await GetImageListByCarId(item.CarId);
                dataLst.Add(new CarDataModel(item.Map(), imageLst.Select(x => new CarImageModel(x.ImageId, x.CarId!, x.ImageName, item.CreatedBy)).ToList()));
            }

            var pageSettingModel = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
            var model = new CarListResponseModel(dataLst, pageSettingModel);

            responseModel = Result<CarListResponseModel>.SuccessResult(model);
        }
        catch (Exception ex)
        {
            responseModel = Result<CarListResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #region Create Car

    public async Task<Result<CarResponseModel>> CreateCar(CarRequestModel requestModel)
    {
        Result<CarResponseModel> responseModel;
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            //if (!await IsAdmin(requestModel.CreatedBy))
            //{
            //    responseModel = GetUserNotFoundResult();
            //    goto result;
            //}

            var carModel = requestModel.Map();
            await _context.TblCars.AddAsync(carModel);
            await _context.SaveChangesAsync();

            var directoryName = GetCarDirectory(requestModel.CreatedBy);

            #region Directory Check / Create

            if (!await DirectoryExists(directoryName))
            {
                if (!await CreateDirectorySuccess(directoryName))
                {
                    responseModel = GetDirectoryFailResult();
                    goto result;
                }
            }

            #endregion

            #region Upload Files & Save

            foreach (var file in requestModel.Files!)
            {
                var fileName = file.GetFileNameV1();
                var imageModel = GetTblImage(carModel.CarId, fileName);

                await _context.TblImages.AddAsync(imageModel);
                await _ftpService.UploadFileAsync(file, directoryName, fileName);
            }

            #endregion

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            responseModel = Result<CarResponseModel>.SuccessResult();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            responseModel = Result<CarResponseModel>.FailureResult(ex);
        }

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
        try
        {
            #region User Not Found

            //if (!await IsAdmin(requestModel.UpdatedBy!))
            //{
            //    responseModel = GetUserNotFoundResult();
            //    goto result;
            //}

            #endregion

            #region Get Active Car By Id

            var item = await GetActiveCarById(carId);
            if (item is null)
            {
                responseModel = GetCarNotFoundResult();
                goto result;
            }

            #endregion

            #region Check Created By & Updated By

            //if (!item.CreatedBy.Equals(item.UpdatedBy))
            //{
            //    responseModel = GetUserNotFoundResult();
            //    goto result;
            //}

            #endregion

            #region Update Entity

            item.Title = requestModel.Title;
            item.Code = requestModel.Code;
            item.Status = requestModel.Status;
            item.Description = requestModel.Description;
            item.Gearbox = requestModel.Gearbox;
            item.SteeringPosition = requestModel.SteeringPosition;
            item.EnginePower = requestModel.EnginePower;
            item.FuelType = requestModel.FuelType;
            item.Mileage = requestModel.Mileage;
            item.Manufacturer = requestModel.Manufacturer;
            item.Model = requestModel.Model;
            item.Year = requestModel.Year;
            item.PlateDivision = requestModel.PlateDivision;
            item.PlateNo = requestModel.PlateNo;
            item.PlateColor = requestModel.PlateColor;
            item.LincenseStatus = requestModel.LincenseStatus;
            item.CarColor = requestModel.CarColor!;
            item.Condition = requestModel.Condition;
            item.Price = requestModel.Price;
            item.SpecialStatus = requestModel.SpecialStatus;
            item.NumberOfViewers = requestModel.NumberOfViewers;
            item.Availability = requestModel.Availability;
            item.IsSold = requestModel.IsSold;
            item.Location = requestModel.Location;
            item.City = requestModel.City;
            item.PaymentOption = requestModel.PaymentOption;
            item.TrimName = requestModel.TrimName;
            item.SellerName = requestModel.SellerName;
            item.PrimaryPhoneNumber = requestModel.PrimaryPhoneNumber;
            item.SecondaryPhoneNumber = requestModel.SecondaryPhoneNumber;
            item.Email = requestModel.Email;
            item.Address = requestModel.Address;
            item.UpdatedBy = requestModel.UpdatedBy;
            item.UpdatedDate = DevCode.GetCurrentMyanmarDateTime();
            item.IsPopular = requestModel.IsPopular;
            item.IsHotDeal = requestModel.IsHotDeal;

            _context.TblCars.Update(item);

            #endregion

            var directoryName = GetCarDirectory(requestModel.CreatedBy!);

            #region Remove Images

            if (requestModel.RemoveImages is not null && requestModel.RemoveImages.Count > 0)
            {
                foreach (var image in requestModel.RemoveImages)
                {
                    var imageModel = await _context.TblImages.FindAsync(image.ImageId);
                    if (imageModel is null)
                    {
                        responseModel = GetImageNotFoundResult();
                        goto result;
                    }

                    _context.TblImages.Remove(imageModel);
                    await _ftpService.DeleteFileAsync(directoryName + "/" + imageModel.ImageName);
                }
            }

            #endregion

            #region Upload Files

            if (requestModel.Files is not null && requestModel.Files.Count > 0)
            {
                foreach (var file in requestModel.Files)
                {
                    var fileName = file.GetFileNameV1();
                    var tblImageModel = GetTblImage(item.CarId, fileName);

                    await _context.TblImages.AddAsync(tblImageModel);

                    bool isDirectoryExist = await _ftpService.CheckDirectoryExistsAsync(directoryName);
                    if (!isDirectoryExist)
                    {
                        bool isDirectoryCreateSuccess = await _ftpService.CreateDirectoryAsync(directoryName);
                        if (!isDirectoryCreateSuccess)
                        {
                            responseModel = Result<CarResponseModel>.FailureResult("Creating Directory Fail.");
                            goto result;
                        }
                    }

                    await _ftpService.UploadFileAsync(file, directoryName, fileName);
                }
            }

            #endregion

            await _context.SaveChangesAsync();
            responseModel = Result<CarResponseModel>.SuccessResult();
        }
        catch (Exception ex)
        {
            responseModel = Result<CarResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Delete Car

    public async Task<Result<CarResponseModel>> DeleteCar(string carId)
    {
        Result<CarResponseModel> responseModel;
        try
        {
            var item = await GetActiveCarById(carId);
            if (item is null)
            {
                responseModel = GetCarNotFoundResult();
                goto result;
            }

            item.IsDeleted = true;
            _context.TblCars.Update(item);

            var imgLstByCar = await _context.TblImages.Where(x => x.CarId == carId).ToListAsync();
            var directoryName = GetCarDirectory(item.CreatedBy);

            if (imgLstByCar is not null && imgLstByCar.Count > 0)
            {
                foreach (var img in imgLstByCar)
                {
                    _context.TblImages.RemoveRange(imgLstByCar);

                    var filePath = directoryName + $"/{img.ImageName}";
                    await _ftpService.DeleteFileAsync(filePath);
                }
            }

            await _context.SaveChangesAsync();
            responseModel = Result<CarResponseModel>.SuccessResult();
        }
        catch (Exception ex)
        {
            responseModel = Result<CarResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    private string GetCarDirectory(string userId) =>
        $"{userId}/{Convert.ToString(EnumFtpFolderName.Car)}";

    private TblImage GetTblImage(string carId, string fileName)
    {
        return new TblImage
        {
            ImageId = Ulid.NewUlid().ToString(),
            CarId = carId,
            ImageName = fileName
        };
    }

    private Result<CarResponseModel> GetCarNotFoundResult() =>
        Result<CarResponseModel>.FailureResult(MessageResource.NotFound, EnumStatusCode.NotFound);

    private async Task<TblCar?> GetActiveCarById(string carId)
    {
        return await _context.TblCars.FirstOrDefaultAsync(x => x.CarId == carId && !x.IsDeleted);
    }

    private async Task<bool> IsAdmin(string userId)
    {
        return await _context.TblUsers.AnyAsync(x =>
            x.UserId == userId && !x.IsDeleted && x.UserRole == Convert.ToString(EnumUserRole.Admin)
        );
    }

    private async Task<bool> DirectoryExists(string directoryName)
    {
        return await _ftpService.CheckDirectoryExistsAsync(directoryName);
    }

    private async Task<bool> CreateDirectorySuccess(string directoryName)
    {
        return await _ftpService.CreateDirectoryAsync(directoryName);
    }

    private async Task<List<TblImage>> GetImageListByCarId(string carId)
    {
        return await _context.TblImages.Where(x => x.CarId == carId).ToListAsync();
    }

    private Result<CarResponseModel> GetUserNotFoundResult() =>
        Result<CarResponseModel>.FailureResult("User Not Found.", EnumStatusCode.NotFound);

    private Result<CarResponseModel> GetImageNotFoundResult() =>
        Result<CarResponseModel>.FailureResult("Image Not Found.", EnumStatusCode.NotFound);

    private Result<CarResponseModel> GetDirectoryFailResult() =>
        Result<CarResponseModel>.FailureResult("Creating Directory Fail.");
}
