using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CarRentalApp.Services;

namespace CarRentalApp.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public MinimumAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        public override bool IsValid(object? value)
        {
            if (value is not DateTime valueAsDateTime)
            {
                return false;
            }

            return UserService.CheckMinimumAge(valueAsDateTime, _minimumAge);
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(
                CultureInfo.CurrentCulture,
                ErrorMessageString,
                name,
                _minimumAge
            );
        }
    }
}