namespace SharmalRealEstateSystem.Shared.Services.ValidationServices.Admin.Auth;

public class UpdateAuthValidator : AbstractValidator<UpdateAuthRequestModel>
{
    public UpdateAuthValidator()
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
        RuleFor(x => x.UserRole)
            .NotNull()
            .WithMessage("User Role cannot be null.")
            .NotEmpty()
            .WithMessage("User Role cannot be empty.");
    }
}
