namespace _03.FlightBookingSystem.API.Helper
{
    /// <summary>
    /// Standard API response wrapper that includes status code, message, and optional errors.
    /// </summary>
    public class ResponseAPI
    {
        /// <summary>
        /// Constructor with status code and optional message.
        /// </summary>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="message">Optional custom message. If null, a default message will be used.</param>
        public ResponseAPI(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageFromStatusCode(statusCode);
        }

        /// <summary>
        /// Constructor with status code, custom message, and error details.
        /// </summary>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="message">Custom message.</param>
        /// <param name="errors">A list of validation or system errors.</param>
        public ResponseAPI(int statusCode, string? message, IEnumerable<string>? errors)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageFromStatusCode(statusCode);
            Errors = errors;
        }

        /// <summary>
        /// Maps standard status codes to default messages.
        /// </summary>
        /// <param name="statusCode">The status code to map.</param>
        /// <returns>Default message for the given status code.</returns>
        private string GetMessageFromStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "Done",
                400 => "Bad Request",
                401 => "Unauthorized",
                404 => "Not Found",
                500 => "Server Error",
                _ => "Unexpected Error"
            };
        }

        /// <summary>
        /// HTTP status code of the response.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Human-readable message about the result.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Optional collection of error details (e.g., validation errors).
        /// </summary>
        public IEnumerable<string>? Errors { get; set; }
    }
}
