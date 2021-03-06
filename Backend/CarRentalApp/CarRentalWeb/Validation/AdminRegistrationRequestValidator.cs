using CarRentalWeb.Models.Requests;
using FluentValidation;
using Microsoft.Extensions.Options;
using SharedResources.Configurations;
using SharedResources.EnumsAndConstants;
using SharedResources.Helpers;

namespace CarRentalWeb.Validation
{
    public class AdminRegistrationRequestValidator : AbstractValidator<AdminRegistrationRequest>
    {
        public AdminRegistrationRequestValidator(IOptions<UserRequirements> userRequirementsOptions)
        {
            var adminMinimumAge = userRequirementsOptions.Value.AdminMinimumAge;

            RuleFor(x => x.DateOfBirth)
                .Must(dateOfBirth => dateOfBirth?.WasYearsAgo(adminMinimumAge) ?? false)
                .WithMessage(String.Format(ValidationConstants.InvalidAgeErrorMessage, adminMinimumAge));
        }
    }
}