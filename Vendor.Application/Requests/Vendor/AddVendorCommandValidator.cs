using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Vendor.Infrastructure.Implementation.Persistence;

namespace Vendor.Application.Requests.Vendor
{
    public class AddVendorCommandValidator : AbstractValidator<AddVendorCommand>
    {
        private readonly IVendorDbContext _dbContext;
        public AddVendorCommandValidator(IVendorDbContext dbContext)
        {
            _dbContext = dbContext;

            // Validates that the Vendor name is unique and not empty and provides an error message if it is.
            // Checks that the State/Province/Region is not empty, with an error message if it is.
            // Ensures that the Country is not empty, with an error message if it is.
            // Checks that the Email is in a valid email address format, with an error message for invalid addresses.
            // Validates that the Phone number matches the international phone number format, with an error message if it does not.
            // Checks that the Service ID is greater than zero, with an error message if it is not.
            // Ensures that the IsApproved field is not null, with an error message if it is.
            // Validates that at least one Market ID is provided, with an error message if none are provided.

            RuleFor(v => v.Name).NotEmpty().WithMessage("Vendor name is required.").Must(name => BeUniqueName(name)).WithMessage("Vendor name must be unique.");
            RuleFor(v => v.Country).NotEmpty().WithMessage("Country is required.");
            RuleFor(v => v.Email).EmailAddress().WithMessage("Invalid email address.");
            RuleFor(v => v.Phone).Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number.");
            RuleFor(v => v.ServiceId).GreaterThan(0).WithMessage("Service ID must be greater than zero.");
            RuleFor(v => v.IsApproved).NotNull().WithMessage("Approval status is required.");
            RuleFor(v => v.MarketIds).NotEmpty().WithMessage("At least one market ID is required.");
        }
        private bool BeUniqueName(string name)
        {
            return !_dbContext.Vendors.Any(v => v.Name == name);
        }
    }
}
