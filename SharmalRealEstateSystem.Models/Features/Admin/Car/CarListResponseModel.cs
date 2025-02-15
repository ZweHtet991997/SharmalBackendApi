namespace SharmalRealEstateSystem.Models.Features.Admin.Car;

public record CarListResponseModel(List<CarDataModel> DataLst, PageSettingModel PageSetting);

public record CarDataModel(CarModel Car, List<CarImageModel> Images);

public record CarImageModel(string ImageId, string CarId, string ImageName,string CreatedBy);
