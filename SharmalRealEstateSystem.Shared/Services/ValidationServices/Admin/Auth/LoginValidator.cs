namespace SharmalRealEstateSystem.Shared.Services.ValidationServices.Admin.Auth;

public class LoginValidator : AbstractValidator<LoginRequestModel>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).EmailAddress().NotEmpty().NotNull().WithMessage("Email is invalid.");
        RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password cannot be empty.");
    }
}
