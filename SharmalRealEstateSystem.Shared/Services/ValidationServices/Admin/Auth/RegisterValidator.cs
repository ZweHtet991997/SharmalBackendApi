namespace SharmalRealEstateSystem.Shared.Services.ValidationServices.Admin.Auth;

public class RegisterValidator : AbstractValidator<RegisterRequestModel>
{
    public RegisterValidator()
    {
        RuleFor(x => x.UserName)
            .NotNull()
            .WithMessage("User Name cannot be null.")
            .NotEmpty()
            .WithMessage("User Name cannot be empty.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email is invalid.")
            .NotNull()
            .WithMessage("Email cannot be null.")
            .NotEmpty()
            .WithMessage("Email cannot be empty.");

        RuleFor(x => x.Password)
            .NotNull()
            .WithMessage("Password cannot be null.")
            .NotEmpty()
            .WithMessage("Password cannot be empty.");

        RuleFor(x => x.UserRole)
            .NotNull()
            .WithMessage("User Role cannot be null.")
            .NotEmpty()
            .WithMessage("User Role cannot be empty.");

        RuleFor(x => x.CreatedBy)
            .NotNull().WithMessage("Created By cannot be null.")
            .NotEmpty().WithMessage("Created By cannot be empty.");
    }
}
