using FluentValidation;

namespace CarMarketApp.Application.DTOs.Adverts;

public sealed class UpdateAdvertDtoValidator : AbstractValidator<UpdateAdvertDto>
{
    public UpdateAdvertDtoValidator()
    {
        RuleFor(a => a.Id).NotEmpty().WithMessage("Id cannot be empty");

        RuleFor(a => a.Price).NotEmpty().WithMessage("Price cannot be empty")
                             .GreaterThan(0).WithMessage("Price must be greater than 0")
                             .LessThanOrEqualTo(10_000_000).WithMessage("Price seems too high");

        RuleFor(a => a.Year).NotEmpty().WithMessage("Year cannot be empty")
                            .InclusiveBetween(1900, DateTimeOffset.UtcNow.Year + 1).WithMessage("Year must be valid");

        RuleFor(a => a.Mileage).NotEmpty().WithMessage("Mileage cannot be empty")
                               .GreaterThanOrEqualTo(0).WithMessage("Mileage must be 0 or more")
                               .LessThanOrEqualTo(1_000_000).WithMessage("Mileage seems too high");

        RuleFor(b => b.Description).Must(d => !string.IsNullOrWhiteSpace(d))
                                   .WithMessage("Description cannot be empty")
                                   .MinimumLength(2).WithMessage("Description must be at least 2 characters long")
                                   .MaximumLength(1000).WithMessage("Description must be at most 1000 characters long");

        RuleFor(b => b.ModelId).NotEmpty().WithMessage("Model id cannot be empty");
    }
}
