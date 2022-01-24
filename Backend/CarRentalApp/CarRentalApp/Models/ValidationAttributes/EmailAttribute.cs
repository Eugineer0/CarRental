using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CarRentalApp.Models.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property |
                    AttributeTargets.Field, AllowMultiple = false
    )]
    public sealed class EmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is not string valueAsString)
            {
                return false;
            }

            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" +
                             @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" +
                             @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$.{3,254}";

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);


            return regex.IsMatch(valueAsString);
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
                ErrorMessageString, name);
        }
    }
}