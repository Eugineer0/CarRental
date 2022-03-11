using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CarRentalBll.Configurations;
using CarRentalBll.Services;
using Microsoft.Extensions.Options;

namespace CarRentalWeb.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class MinimumAgeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var minimumAge = ((IOptions<UserRequirements>) validationContext.GetService(typeof(IOptions<UserRequirements>))).Value.AdminMinimumAge;

            if (value is not DateTime valueAsDateTime)
            {
                return new ValidationResult(ErrorMessage);
            }

            return UserService.CheckIfHasAge(valueAsDateTime, minimumAge) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(
                CultureInfo.CurrentCulture,
                ErrorMessageString,
                name
            );
        }
    }
}