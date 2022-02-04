namespace CarRentalApp.Exceptions
{
    public class GeneralException : Exception
    {
        public ErrorTypes ErrorType { get; set; }

        public GeneralException(ErrorTypes errorType, string? message, Exception? innerException)
            : base(message, innerException)
        {
            ErrorType = errorType;
        }

   
    }

    public enum ErrorTypes
    {
        AuthFailed,
        Invalid,
        NotFound,
        Conflict
    }
}