using FluentValidation;

namespace Vendor.Application.Requests.Markets
{
    public class AddMarketCommandValidator:AbstractValidator<AddMarketCommand>
    {
        // Constructor for AddMarketCommandValidator
        public AddMarketCommandValidator()
        {
            // 1. Validates that the Name property of AddMarketCommand is not empty.
            // 2. Provides a custom error message if the Name property is empty.
            RuleFor(v => v.Name).NotEmpty().WithMessage("Market name is required.");
        }

    }
}
