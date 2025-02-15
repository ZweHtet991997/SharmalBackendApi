namespace SharmalRealEstateSystem.Models.Features.Admin.AdsPage;

public class AdsPageListResponseModel
{
    public List<AdsPageModel> DataLst { get; set; }

    public AdsPageListResponseModel(List<AdsPageModel> dataLst)
    {
        DataLst = dataLst;
    }

    public AdsPageListResponseModel() { }
}
