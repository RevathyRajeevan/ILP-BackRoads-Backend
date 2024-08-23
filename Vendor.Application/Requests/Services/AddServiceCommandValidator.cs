using FluentValidation;

namespace Vendor.Application.Requests.Services
{
    internal class AddServiceCommandValidator:AbstractValidator<AddServiceCommand>
    {
        // Constructor for AddServiceCommandValidator
        public AddServiceCommandValidator()
        {
            // 1. Validates that the Name property of AddServiceCommand is not empty.
            // 2. Provides a custom error message if the Name property is empty.
            RuleFor(v => v.Name).NotEmpty().WithMessage("Service name is required.");
        }
    }
}
