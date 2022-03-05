namespace CarRentalWeb.Validation
{
    public static class ValidationConstants
    {
        public const string DriverLicenseRegExp = "[0-9]{1}[A-Z]{2}[0-9]{6}";
        public const string DriverLicenseErrorMessage = "Incorrect format: The {0} value must consist of 1 digit leading 2 capitals, followed by 6 digits";
        public const string StringLengthErrorMessage = "Incorrect format: The {0} value must have length between {2} and {1} characters";
    }
}