using _01.FlightBookingSystem.Core.DTO_s.Booking;
using _01.FlightBookingSystem.Core.DTO_s.Payment;
using _01.FlightBookingSystem.Core.Services;
using _03.FlightBookingSystem.API.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _03.FlightBookingSystem.API.Controllers
{
    [Authorize]
    public class BookingController : BaseController
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService, IMapper mapper) : base(mapper)
        {
            _bookingService = bookingService;
        }

        // Get a specific Booking by its ID
        [HttpGet("GetByID/{ID}", Name = "GetBookingByID")]
        public async Task<IActionResult> GetBookingByID(int ID)
        {
            try
            {
                if (ID <= 0)
                    return BadRequest(new ResponseAPI(400, "Invalid ID."));
                var userId = GetCurrentUserId();
                var booking = await _bookingService.GetBookingByIDAsync(ID, GetCurrentRoleForUser(), userId);

                if (booking == null)
                    return NotFound(new ResponseAPI(404, "Book not found."));

                var bookingDTO = _mapper.Map<List<BookingDTO>>(booking);
                return Ok(bookingDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI(500, $"Internal server error: {ex.Message}"));
            }
        }

        // Book a seat for the currently logged-in user
        [HttpPost("BookSeat")]
        public async Task<IActionResult> BookSeat(int seatId)
        {
            try
            {
                if (seatId <= 0)
                    return BadRequest(new ResponseAPI(400, "Please enter a valid SeatID."));

                var userId = GetCurrentUserId();
                var result = await _bookingService.BookSeatAsync(userId, seatId);

                return result.Succeeded ? Ok(result) : Conflict(result);
            }
            catch (Exception ex) { 
            return StatusCode(500,new ResponseAPI(500,$"Internal server error: {ex.Message}"));
            }
        }

        // Cancel a booking for the currently logged-in user
        [HttpDelete("CancelBooking")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            try
            {
                if (bookingId <= 0)
                    return BadRequest(new ResponseAPI(400, "Please enter a valid BookingID."));

                var userId = GetCurrentUserId();
                var result = await _bookingService.CancelBookingAsync(userId, GetCurrentRoleForUser(), bookingId);

                return result.Succeeded ? Ok(result) : Conflict(result);
            }
            catch (Exception ex) {
                return StatusCode(500, new ResponseAPI(500, $"Internal server error: {ex.Message}"));
            }
        }

        // Get a list of all bookings
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllBookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingsAsync();
                if (bookings == null || bookings.Count == 0)
                    return NotFound(new ResponseAPI(404, "No bookings found."));

                var bookingsDTO = _mapper.Map<List<BookingDTO>>(bookings);
                return Ok(bookingsDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, new ResponseAPI(500, $"Internal server error: {ex.Message}"));
            }
        }

        // Confirm a payment using payment intent ID
        [HttpPost("ConfirmPayment")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequestDTO confirmPayment)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(confirmPayment.PaymentIntentId) || string.IsNullOrWhiteSpace(confirmPayment.PaymentMethodId))
                    return BadRequest(new ResponseAPI(400, "PaymentIntent ID and PaymentMethod ID cannot be empty."));

                var result = await _bookingService.ConfirmBookingPaymentAsync(confirmPayment.PaymentIntentId, confirmPayment.PaymentMethodId);
                return result.Succeeded ? Ok(result) : Conflict(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI(500, $"Internal server error: {ex.Message}"));
            }
        }

        // Helper method to get current user ID from token
        private string GetCurrentUserId()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }
        // Helper method to get current user Role from token
        private string GetCurrentRoleForUser()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        }
    }
}
