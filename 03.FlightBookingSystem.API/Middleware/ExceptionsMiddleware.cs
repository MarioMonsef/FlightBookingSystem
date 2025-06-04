using _03.FlightBookingSystem.API.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace _03.FlightBookingSystem.API.Middleware
{
    /// <summary>
    /// Middleware for handling exceptions and applying basic IP-based rate limiting.
    /// </summary>
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionsMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="environment">Provides information about the web hosting environment.</param>
        /// <param name="cache">In-memory cache used for rate limiting.</param>
        public ExceptionsMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache cache)
        {
            _next = next;
            _environment = environment;
            _cache = cache;
        }

        /// <summary>
        /// Processes HTTP requests, applies security headers, rate limiting, and handles exceptions.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                ApplySecurity(context);

                if (!IsRequestAllowed(context))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var response = new APIExceptions((int)HttpStatusCode.TooManyRequests, "Too many requests, please try again later.");
                    await context.Response.WriteAsJsonAsync(response);
                    //return; // Prevents further processing if rate limited
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = _environment.IsDevelopment()
                    ? new APIExceptions((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    : new APIExceptions((int)HttpStatusCode.InternalServerError, ex.Message);

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }

        /// <summary>
        /// Checks whether the request from a given IP is allowed based on a fixed window rate limit.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <returns>True if the request is within the allowed rate; otherwise, false.</returns>
        private bool IsRequestAllowed(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress.ToString();
            var timeNow = DateTime.UtcNow;
            var cacheKey = $"Rate:{ip}";

            // Get or create a cache entry for the current IP address
            var (timestamp, count) = _cache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return (Timestamp: timeNow, count: 0);
            });

            if (timeNow - timestamp < _rateLimitWindow)
            {
                if (count >= 8)
                {
                    return false;
                }

                _cache.Set(cacheKey, (timestamp, count + 1), _rateLimitWindow);
            }

            return true;
        }

        /// <summary>
        /// Adds basic security headers to the HTTP response.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-XSS-Protection"] = "1;mode=block";
            context.Response.Headers["X-Frame-Options"] = "DENY";
        }
    }
}
