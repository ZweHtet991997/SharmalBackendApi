namespace SharmalRealEstateSystem.Shared.Services.ValidationServices.Admin.Property;

public class UpdatePropertyValidator : AbstractValidator<UpdatePropertyRequestModel>
{
    public UpdatePropertyValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .WithMessage("Property Title cannot be null.")
            .NotEmpty()
            .WithMessage("Property Title cannot be empty.");

        RuleFor(x => x.Status)
            .NotNull()
            .WithMessage("Property Status cannot be null.")
            .NotEmpty()
            .WithMessage("Status cannot be empty.");

        RuleFor(x => x.Type)
            .NotNull()
            .WithMessage("Property Type cannot be null.")
            .NotEmpty()
            .WithMessage("Property Type cannot be empty");

        RuleFor(x => x.Price)
            .NotNull()
            .WithMessage("Price cannot be null.")
            .NotEmpty()
            .WithMessage("Price cannot be empty.")
            .GreaterThan(0)
            .WithMessage("Price is invalid.");

        RuleFor(x => x.PaymentOption)
            .NotNull()
            .WithMessage("Payment Option cannot be null")
            .NotEmpty()
            .WithMessage("Payment Option cannot be empty.");

        RuleFor(x => x.Location)
            .NotNull()
            .WithMessage("Location cannot be null.")
            .NotEmpty()
            .WithMessage("Location cannot be empty.");

        RuleFor(x => x.City)
            .NotNull()
            .WithMessage("City cannot be null.")
            .NotEmpty()
            .WithMessage("City cannot be empty.");

        //RuleFor(x => x.NumberOfViewers)
        //    .NotNull()
        //    .WithMessage("Number Of Viewers cannot be null")
        //    .NotEmpty()
        //    .WithMessage("Number Of Viewers cannot be empty.");

        //RuleFor(x => x.Bedrooms)
        //    .NotNull()
        //    .WithMessage("Bedroom cannot be null.")
        //    .NotEmpty()
        //    .WithMessage("Bedroom cannot be empty.")
        //    .GreaterThan(0)
        //    .WithMessage("Bedroom is invalid.");

        RuleFor(x => x.Area)
            .NotNull()
            .WithMessage("Area cannot be null.")
            .NotEmpty()
            .WithMessage("Area cannot be empty.");

        RuleFor(x => x.Condition)
            .NotNull()
            .WithMessage("Condition cannot be null.")
            .NotEmpty()
            .WithMessage("Condition cannot be empty.");

        RuleFor(x => x.Description)
            .NotNull()
            .WithMessage("Description cannot be null.")
            .NotEmpty()
            .WithMessage("Description cannot be empty.");

        //RuleFor(x => x.Furnished)
        //    .NotNull()
        //    .WithMessage("Furnished cannot be null.")
        //    .NotEmpty()
        //    .WithMessage("Furnished cannot be empty.");

        RuleFor(x => x.SellerName)
            .NotNull()
            .WithMessage("Seller Name cannot be null.")
            .NotEmpty()
            .WithMessage("Seller Name cannot be empty.");

        RuleFor(x => x.PrimaryPhoneNumber)
            .NotNull()
            .WithMessage("Primary Phone Number cannot be null.")
            .NotEmpty()
            .WithMessage("Primary Phone Number cannot be empty.")
            .MaximumLength(11)
            .WithMessage("Primary Phone Number maximum length is 11.")
            .MinimumLength(11)
            .WithMessage("Primary Phone Number minimum length is 11.");

        RuleFor(x => x.UpdatedBy)
            .NotNull()
            .WithMessage("Updated By cannot be null")
            .NotEmpty()
            .WithMessage("Updated By cannot be empty.");
    }
}
