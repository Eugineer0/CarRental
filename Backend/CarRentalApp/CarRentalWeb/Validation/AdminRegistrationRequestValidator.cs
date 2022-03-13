using CarRentalWeb.Models.Requests;
using FluentValidation;
using Microsoft.Extensions.Options;
using SharedResources.Configurations;
using SharedResources.Helpers;

namespace CarRentalWeb.Validation
{
    public class AdminRegistrationRequestValidator : AbstractValidator<AdminRegistrationRequest>
    {
        public AdminRegistrationRequestValidator(IOptions<UserRequirements> userRequirementsOptions)
        {
            var adminMinimimAge = userRequirementsOptions.Value.AdminMinimumAge;

            RuleFor(x => x.DateOfBirth)
                .Must(
                    dateOfBirth =>
                    {
                        return DateOperations.CheckMinimumAge((DateTime) dateOfBirth, adminMinimimAge);
                    }
                )
                .WithMessage(String.Format(ValidationConstants.InvalidAgeErrorMessage, adminMinimimAge));
        }
    }
}