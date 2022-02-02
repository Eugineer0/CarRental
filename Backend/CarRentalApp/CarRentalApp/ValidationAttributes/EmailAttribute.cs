using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CarRentalApp.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class EmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is not string valueAsString)
            {
                return false;
            }

            var pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(valueAsString);
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