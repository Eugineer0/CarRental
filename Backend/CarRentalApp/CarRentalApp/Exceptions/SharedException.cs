namespace CarRentalApp.Exceptions
{
    public class SharedException : Exception
    {
        public ErrorTypes ErrorType { get; set; }
        
        public string? DeveloperInfo { get; set; }

        public SharedException(ErrorTypes errorType, string message)
            : base(message, null)
        {
            ErrorType = errorType;
            DeveloperInfo = null;
        }
        
        public SharedException(ErrorTypes errorType, string message, string developerInfo)
            : base(message, null)
        {
            ErrorType = errorType;
            DeveloperInfo = developerInfo;
        }

        public SharedException(ErrorTypes errorType, string message, string developerInfo, Exception? innerException)
            : base(message, innerException)
        {
            ErrorType = errorType;
            DeveloperInfo = developerInfo;
        }
    }

    public enum ErrorTypes
    {
        NotEnoughData,
        AuthFailed,
        AccessDenied,
        Invalid,
        NotFound,
        Conflict
    }
}