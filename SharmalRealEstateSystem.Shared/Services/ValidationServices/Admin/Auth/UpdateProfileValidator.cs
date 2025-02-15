namespace SharmalRealEstateSystem.Shared.Services.ValidationServices.Admin.Auth;

public class UpdateProfileValidator : AbstractValidator<UpdateProfileRequestModel>
{
    public UpdateProfileValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("User Name cannot be empty.")
            .NotNull()
            .WithMessage("User Name cannot be null.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email format is invalid.")
            .NotEmpty()
            .WithMessage("Email cannot be empty.")
            .NotNull()
            .WithMessage("Email cannot be null.");

        RuleFor(x => x.UserRole)
            .NotEmpty()
            .WithMessage("User Role cannot empty.")
            .NotNull()
            .WithMessage("User Role cannot be null.");
    }
}
