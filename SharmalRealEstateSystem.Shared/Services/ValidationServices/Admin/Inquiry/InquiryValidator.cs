namespace SharmalRealEstateSystem.Shared.Services.ValidationServices.Admin.Inquiry;

public class InquiryValidator : AbstractValidator<InquiryRequestModel>
{
    public InquiryValidator()
    {
        RuleFor(x => x.UserName)
            .NotNull()
            .WithMessage("User Name cannot be null.")
            .NotEmpty()
            .WithMessage("User Name cannot be empty.");

        RuleFor(x => x.PhoneNumber)
            .NotNull()
            .WithMessage("Phone Number cannot be null.")
            .NotEmpty()
            .WithMessage("Phone Number cannot be empty.")
            .MaximumLength(11)
            .WithMessage("Phone Number max length is 11.")
            .MinimumLength(11)
            .WithMessage("Phone Number min length is 11.");

        RuleFor(x => x.Email)
            .NotNull()
            .WithMessage("Email cannot be null.")
            .NotEmpty()
            .WithMessage("Email cannot be empty.")
            .EmailAddress()
            .WithMessage("Email is invalid.");

        RuleFor(x => x.Description)
            .NotNull()
            .WithMessage("Description cannot be null.")
            .NotEmpty()
            .WithMessage("Description cannot be empty.");
    }
}
