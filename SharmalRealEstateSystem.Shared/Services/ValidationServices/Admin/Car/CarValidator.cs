namespace SharmalRealEstateSystem.Shared.Services.ValidationServices.Admin.Car;

public class CarValidator : AbstractValidator<CarRequestModel>
{
    public CarValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .WithMessage("Title cannot be null.")
            .NotEmpty()
            .WithMessage("Title cannot be empty.");

        RuleFor(x => x.Description)
            .NotNull()
            .WithMessage("Description cannot be null")
            .NotEmpty()
            .WithMessage("Description cannot be empty.");

        RuleFor(x => x.SteeringPosition)
            .NotNull()
            .WithMessage("Steering Position cannot be null.")
            .NotEmpty()
            .WithMessage("Steering Position cannot be empty.");

        RuleFor(x => x.EnginePower)
            .NotNull()
            .WithMessage("Engine Power cannot be null.")
            .NotEmpty()
            .WithMessage("Engine Power cannot be empty.");

        RuleFor(x => x.Manufacturer)
            .NotNull()
            .WithMessage("Manufacturer cannot be null.")
            .NotEmpty()
            .WithMessage("Manufacturer cannot be null.");

        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Model cannot be null.")
            .NotEmpty()
            .WithMessage("Model cannot be empty.");

        RuleFor(x => x.Year)
            .NotNull()
            .WithMessage("Year cannot be null.")
            .NotEmpty()
            .WithMessage("Year cannot be empty.");

        RuleFor(x => x.CarColor)
            .NotNull()
            .WithMessage("Car Color cannot be null.")
            .NotEmpty()
            .WithMessage("Car Color cannot be empty.");

        RuleFor(x => x.Condition)
            .NotNull()
            .WithMessage("Condition cannot be null.")
            .NotEmpty()
            .WithMessage("Condition cannot be empty.");

        RuleFor(x => x.Price)
            .NotNull()
            .WithMessage("Price cannot be null.")
            .NotEmpty()
            .WithMessage("Price cannot be empty.");

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

        RuleFor(x => x.SellerName)
            .NotNull()
            .WithMessage("Seller Name cannot be null.")
            .NotEmpty()
            .WithMessage("Seller Name cannot be empty.");

        RuleFor(x => x.PrimaryPhoneNumber)
            .NotNull()
            .WithMessage("Primary Phone Number Name cannot be null.")
            .NotEmpty()
            .WithMessage("Primary Phone Number cannot be empty.");

        RuleFor(x => x.CreatedBy)
            .NotNull()
            .WithMessage("Created By cannot be null.")
            .NotEmpty()
            .WithMessage("Created By cannot be empty.");
    }
}
