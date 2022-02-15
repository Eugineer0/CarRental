using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CarRentalApp.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ForbidFillInAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            return value == null;
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