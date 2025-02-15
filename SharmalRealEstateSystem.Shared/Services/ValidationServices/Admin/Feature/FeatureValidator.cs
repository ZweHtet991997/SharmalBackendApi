namespace SharmalRealEstateSystem.Shared.Services.ValidationServices.Admin.Feature;

public class FeatureValidator : AbstractValidator<FeatureRequestModel>
{
    public FeatureValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage("Feature Name cannot be null.")
            .NotEmpty()
            .WithMessage("Feature Name cannot be empty.");
    }
}
