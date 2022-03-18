using CarRentalWeb.Models.Requests;
using FluentValidation;
using Microsoft.Extensions.Options;
using SharedResources.Configurations;
using SharedResources.EnumsAndConstants;
using SharedResources.Helpers;

namespace CarRentalWeb.Validation
{
    public class ClientRegistrationRequestValidator : AbstractValidator<ClientRegistrationRequest>
    {
        public ClientRegistrationRequestValidator(IOptions<UserRequirements> userRequirementsOptions)
        {
            var clientMinimimAge = userRequirementsOptions.Value.ClientMinimumAge;

            RuleFor(x => x.DateOfBirth)
                .Must(dateOfBirth => dateOfBirth?.WasYearsAgo(clientMinimimAge) ?? false)
                .WithMessage(String.Format(ValidationConstants.InvalidAgeErrorMessage, clientMinimimAge));
        }
    }
}