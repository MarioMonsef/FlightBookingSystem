namespace _03.FlightBookingSystem.API.Helper
{
    /// <summary>
    /// Custom exception response used for structured error handling in API responses.
    /// Inherits from ResponseAPI and adds optional technical details for debugging.
    /// </summary>
    public class APIExceptions : ResponseAPI
    {
        /// <summary>
        /// Optional stack trace or technical details for debugging (used mostly in development).
        /// </summary>
        public string? Details { get; set; }

        /// <summary>
        /// Constructs an API exception with status code, message, optional technical details, and errors.
        /// </summary>
        /// <param name="statusCode">HTTP status code.</param>
        /// <param name="message">Optional user-facing message.</param>
        /// <param name="details">Optional technical details like stack trace.</param>
        /// <param name="errors">Optional list of specific errors (e.g., validation messages).</param>
        public APIExceptions(int statusCode, string? message = null, string? details = null, IEnumerable<string>? errors = null)
            : base(statusCode, message, errors)
        {
            Details = details;
        }
    }
}
