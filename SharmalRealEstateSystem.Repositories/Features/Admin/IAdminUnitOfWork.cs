namespace SharmalRealEstateSystem.Repositories.Features.Admin;

public interface IAdminUnitOfWork
{
    IAuthRepository AuthRepository { get; }
    IExchangeRateRepository ExchangeRateRepository { get; }
    IPropertyRepository PropertyRepository { get; }
    IFeatureRepository FeatureRepository { get; }
    IInquiryRepository InquiryRepository { get; }
    IAdsPageRepository AdsPageRepository { get; }
    IAdsRepository AdsRepository { get; }
    ICarRepository CarRepository { get; }
}
