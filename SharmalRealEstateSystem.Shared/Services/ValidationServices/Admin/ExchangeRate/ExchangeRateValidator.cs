namespace SharmalRealEstateSystem.Shared.Services.ValidationServices.Admin.ExchangeRate;

public class ExchangeRateValidator : AbstractValidator<ExchangeRateRequestModel>
{
    public ExchangeRateValidator()
    {
        RuleFor(x => x.Currency)
            .NotNull()
            .WithMessage("Currency cannot be null.")
            .NotEmpty()
            .WithMessage("Currency cannot be empty.");
        RuleFor(x => x.ExchangeRate).GreaterThan(0).WithMessage("Exchange Rate is invalid");
    }
}
