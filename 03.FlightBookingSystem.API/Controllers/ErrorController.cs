using _03.FlightBookingSystem.API.Helper;
using Microsoft.AspNetCore.Mvc;

namespace _03.FlightBookingSystem.API.Controllers
{
    [Route("errors/{statusCode}")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        // Returns a structured error response based on the status code (used by middleware for error redirection)
        [HttpGet]
        public IActionResult Error(int statusCode)
        {
            return new ObjectResult(new ResponseAPI(statusCode));
        }
    }
}
