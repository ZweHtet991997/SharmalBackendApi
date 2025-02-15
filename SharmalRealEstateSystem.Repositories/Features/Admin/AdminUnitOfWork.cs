namespace SharmalRealEstateSystem.Repositories.Features.Admin;

public class AdminUnitOfWork : IAdminUnitOfWork
{
    private readonly IServiceProvider _serviceProvider;
    private IExchangeRateRepository _exchangeRateRepository;
    private IAuthRepository _authRepository;
    private IPropertyRepository _propertyRepository;
    private IFeatureRepository _featureRepository;
    private IInquiryRepository _inquiryRepository;
    private IAdsPageRepository _adsPageRepository;
    private IAdsRepository _adsRepository;
    private ICarRepository _carRepository;

    public AdminUnitOfWork(
        IServiceProvider serviceProvider,
        IAuthRepository authRepository,
        IExchangeRateRepository exchangeRateRepository,
        IPropertyRepository propertyRepository,
        IFeatureRepository featureRepository,
        IInquiryRepository inquiryRepository,
        IAdsPageRepository adsPageRepository,
        IAdsRepository adsRepository,
        ICarRepository carRepository
    )
    {
        _serviceProvider = serviceProvider;
        _authRepository = authRepository;
        _exchangeRateRepository = exchangeRateRepository;
        _propertyRepository = propertyRepository;
        _featureRepository = featureRepository;
        _inquiryRepository = inquiryRepository;
        _adsPageRepository = adsPageRepository;
        _adsRepository = adsRepository;
        _carRepository = carRepository;
    }

    public IAuthRepository AuthRepository =>
        _authRepository ??= _serviceProvider.GetRequiredService<IAuthRepository>();

    public IExchangeRateRepository ExchangeRateRepository =>
        _exchangeRateRepository ??= _serviceProvider.GetRequiredService<IExchangeRateRepository>();

    public IPropertyRepository PropertyRepository =>
        _propertyRepository ??= _serviceProvider.GetRequiredService<IPropertyRepository>();

    public IFeatureRepository FeatureRepository =>
        _featureRepository ??= _serviceProvider.GetRequiredService<IFeatureRepository>();

    public IInquiryRepository InquiryRepository =>
        _inquiryRepository ??= _serviceProvider.GetRequiredService<IInquiryRepository>();

    public IAdsPageRepository AdsPageRepository =>
        _adsPageRepository ??= _serviceProvider.GetRequiredService<IAdsPageRepository>();

    public IAdsRepository AdsRepository =>
        _adsRepository ??= _serviceProvider.GetRequiredService<IAdsRepository>();

    public ICarRepository CarRepository =>
        _carRepository ??= _serviceProvider.GetRequiredService<ICarRepository>();
}
