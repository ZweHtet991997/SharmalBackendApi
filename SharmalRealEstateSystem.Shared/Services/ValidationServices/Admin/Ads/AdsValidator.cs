namespace SharmalRealEstateSystem.Shared.Services.ValidationServices.Admin.Ads;

public class AdsValidator : AbstractValidator<AdsRequestModel>
{
    public AdsValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .WithMessage("Title cannot be null.")
            .NotEmpty()
            .WithMessage("Title cannot be empty.");

        RuleFor(x => x.TargetUrl)
            .NotNull()
            .WithMessage("Target Url cannot be null.")
            .NotEmpty()
            .WithMessage("Target Url cannot be empty.");

        RuleFor(x => x.AdsLayout)
            .NotNull()
            .WithMessage("Ads Layout cannot be null.")
            .NotEmpty()
            .WithMessage("Ads Layout cannot be empty.");

        RuleFor(x => x.StartDate)
            .NotNull()
            .WithMessage("Start Date cannot be null.")
            .NotEmpty()
            .WithMessage("Start Date cannot be empty.");

        RuleFor(x => x.EndDate)
            .NotNull()
            .WithMessage("End Date cannot be null.")
            .NotEmpty()
            .WithMessage("End Date cannot be empty.");

        RuleFor(x => x.CreatedBy)
            .NotNull()
            .WithMessage("Created By cannot be null.")
            .NotEmpty()
            .WithMessage("Created By cannot be empty.");
    }
}
