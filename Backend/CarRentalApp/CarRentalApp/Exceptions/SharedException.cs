namespace CarRentalApp.Exceptions
{
    public class SharedException : Exception
    {
        public ErrorTypes ErrorType { get; set; }
        
        public string? DeveloperExceptionMessage { get; set; }

        public SharedException(ErrorTypes errorType, string message)
            : base(message, null)
        {
            ErrorType = errorType;
            DeveloperExceptionMessage = null;
        }
        
        public SharedException(ErrorTypes errorType, string message, string developerExceptionMessage)
            : base(message, null)
        {
            ErrorType = errorType;
            DeveloperExceptionMessage = developerExceptionMessage;
        }

        public SharedException(ErrorTypes errorType, string message, string developerExceptionMessage, Exception? innerException)
            : base(message, innerException)
        {
            ErrorType = errorType;
            DeveloperExceptionMessage = developerExceptionMessage;
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