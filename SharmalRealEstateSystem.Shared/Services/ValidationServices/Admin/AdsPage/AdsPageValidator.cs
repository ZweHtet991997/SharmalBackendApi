namespace SharmalRealEstateSystem.Shared.Services.ValidationServices.Admin.AdsPage;

public class AdsPageValidator : AbstractValidator<AdsPageRequestModel>
{
    public AdsPageValidator()
    {
        RuleFor(x => x.Pages)
            .NotNull()
            .WithMessage("Page Name cannot be null.")
            .NotEmpty()
            .WithMessage("Page Name cannot be empty.");
    }
}
