using _01.FlightBookingSystem.Core.DTO_s.Booking;
using _01.FlightBookingSystem.Core.Interfaces;
using _01.FlightBookingSystem.Core.Models.Booking;
using _01.FlightBookingSystem.Core.Models.Flight;
using _01.FlightBookingSystem.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace _02.FlightBookingSystem.EF.Services
{
    /// <summary>
    /// Service to manage booking operations including booking seats, cancelling bookings,
    /// confirming payments, and retrieving booking data.
    /// </summary>
    public class BookingService : BaseService, IBookingService
    {
        private readonly ISeatNotifier _seatNotifier;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BookingService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingService"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work for database operations.</param>
        /// <param name="seatNotifier">Notifier for seat updates.</param>
        /// <param name="configuration">Configuration settings.</param>
        /// <param name="logger">Logger for tracking service operations.</param>
        public BookingService(IUnitOfWork unitOfWork, ISeatNotifier seatNotifier, IConfiguration configuration, ILogger<BookingService> logger) : base(unitOfWork)
        {
            _seatNotifier = seatNotifier;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Books a seat for a user and initiates payment intent.
        /// </summary>
        /// <param name="userId">The user ID who is booking.</param>
        /// <param name="seatId">The seat ID to book.</param>
        /// <returns>A <see cref="BookingResultDTO"/> indicating success or failure and payment info.</returns>
        public async Task<BookingResultDTO> BookSeatAsync(string userId, int seatId)
        {
            if (seatId <= 0 || string.IsNullOrWhiteSpace(userId))
            {
                return new BookingResultDTO { Succeeded = false, Massage = "Please enter valid SeatID and UserID." };
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var seat = await _unitOfWork.SeatReposatory.GetByIDAsync(seatId, s => s.Flight);
                if (seat == null || seat.IsBooking)
                {
                    return new BookingResultDTO { Succeeded = false, Massage = "Seat not found or already booked." };
                }

                var paymentService = new PaymentService(_configuration);
                var paymentIntent = await paymentService.CreatePaymentIntent(seat.Flight.Price);

                seat.IsBooking = true;
                var booking = new Booking
                {
                    SeatID = seatId,
                    UserID = userId,
                    StripePaymentIntentId = paymentIntent.Id,
                    PaymentStatus = PaymentStatus.Pending
                };

                await _unitOfWork.BookingReposatory.AddAsync(booking);
                await _unitOfWork.Complete();
                await transaction.CommitAsync();

                await _seatNotifier.NotifySeatUpdate(seat.ID, true);
                _logger.LogInformation("Booking succeeded for SeatID {SeatId} by UserID {UserId}", seatId, userId);

                return new BookingResultDTO { Succeeded = true, Massage = "Booking is done.", ClientSecret = paymentIntent.ClientSecret };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error booking seat with SeatID {SeatId} by UserID {UserId}", seatId, userId);
                return new BookingResultDTO { Succeeded = false, Massage = ex.Message };
            }
        }

        /// <summary>
        /// Cancels an existing booking based on the user identity and role.
        /// </summary>
        /// <param name="userId">The ID of the user attempting to cancel the booking.</param>
        /// <param name="role">The role of the user (e.g., Admin, User) to determine cancellation permissions.</param>
        /// <param name="bookingId">The ID of the booking to be canceled.</param>
        /// <returns>A <see cref="BookingResultDTO"/> indicating the result of the cancellation attempt.</returns>
        public async Task<BookingResultDTO> CancelBookingAsync(string userId, string role, int bookingId)
        {
            if (bookingId <= 0 || string.IsNullOrWhiteSpace(userId))
            {
                return new BookingResultDTO { Succeeded = false, Massage = "Please enter valid BookingID and UserID." };
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var booking = await _unitOfWork.BookingReposatory.GetByIDAsync(bookingId, b => b.Seat);
                if (booking == null)
                {
                    return new BookingResultDTO { Succeeded = false, Massage = "Booking not found. Please check the BookingID." };
                }
                if (booking.UserID != userId && role != "Admin")
                {
                    return new BookingResultDTO { Succeeded = false, Massage = "Only the booking user can cancel." };
                }

                booking.Seat.IsBooking = false;
                await _unitOfWork.BookingReposatory.DeleteAsync(booking.ID);

                if (booking.PaymentStatus == PaymentStatus.Paid)
                {
                    var paymentService = new PaymentService(_configuration);
                    await paymentService.CreateRefund(booking.StripePaymentIntentId);
                }

                await _unitOfWork.Complete();
                await transaction.CommitAsync();

                await _seatNotifier.NotifySeatUpdate(booking.SeatID, false);
                _logger.LogInformation("Booking cancelled for BookingID {BookingId} by UserID {UserId}", bookingId, userId);

                return new BookingResultDTO { Succeeded = true, Massage = "Booking is cancelled." };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error cancelling booking with BookingID {BookingId} by UserID {UserId}", bookingId, userId);
                return new BookingResultDTO { Succeeded = false, Massage = ex.Message };
            }
        }

        /// <summary>
        /// Gets all bookings with related seat and user details.
        /// </summary>
        /// <returns>A list of all bookings or an empty list if none exist.</returns>
        public async Task<IReadOnlyList<Booking>> GetAllBookingsAsync()
        {
            var count = await _unitOfWork.BookingReposatory.Count();
            if (count == 0)
            {
                return new List<Booking>();
            }

            var bookings = await _unitOfWork.BookingReposatory.GetAllAsync(b => b.Seat, b => b.ApplicationUser);
            return bookings;
        }

        /// <summary>
        /// Confirms the payment of a booking by confirming a Stripe PaymentIntent.
        /// </summary>
        /// <param name="paymentIntentId">The Stripe PaymentIntent identifier to confirm.</param>
        /// <param name="PaymentMethodId">The Stripe PaymentMethod identifier to use for confirming the payment.</param>
        /// <returns>A <see cref="BookingResultDTO"/> indicating the success or failure of the payment confirmation.</returns>
        public async Task<BookingResultDTO> ConfirmBookingPaymentAsync(string paymentIntentId,string PaymentMethodId)
        {
            if (string.IsNullOrWhiteSpace(paymentIntentId))
            {
                return new BookingResultDTO { Succeeded = false, Massage = "Please enter a valid paymentIntentId." };
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var booking = await _unitOfWork.BookingReposatory.GetByPaymentIntintID(paymentIntentId);
                if (booking == null)
                {
                    return new BookingResultDTO { Succeeded = false, Massage = "Booking not found for this payment." };
                }

                var paymentService = new PaymentService(_configuration);
                var confirmedPayment = await paymentService.ConfirmPaymentIntent(paymentIntentId, PaymentMethodId);

                if (confirmedPayment.Status != "succeeded")
                {
                    await transaction.RollbackAsync();
                    return new BookingResultDTO { Succeeded = false, Massage = "Payment confirmation failed." };
                }

                booking.PaymentStatus = PaymentStatus.Paid;
                await _unitOfWork.Complete();
                await transaction.CommitAsync();

                _logger.LogInformation("Payment confirmed for BookingID {BookingId} with PaymentIntent {PaymentIntentId}", booking.ID, paymentIntentId);

                return new BookingResultDTO { Succeeded = true, Massage = "Payment confirmed successfully." };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error confirming payment with PaymentIntent {PaymentIntentId}", paymentIntentId);
                return new BookingResultDTO { Succeeded = false, Massage = ex.Message };
            }
        }
        /// <summary>
        /// Retrieves a Booking by its unique identifier if the requesting user has permission.
        /// </summary>
        /// <param name="id">The ID of the Booking to retrieve. Must be greater than zero.</param>
        /// <param name="role">The role of the requesting user (e.g., "Admin").</param>
        /// <param name="userId">The ID of the requesting user.</param>
        /// <returns>
        /// The Booking entity with the specified ID if found and accessible by the user;
        /// otherwise, returns null (if not found, invalid ID, or access denied).
        /// </returns>
        public async Task<Booking> GetBookingByIDAsync(int id, string role, string userId)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid Booking ID received: {Id}", id);
                return null;
            }

            try
            {
                var booking = await _unitOfWork.BookingReposatory.GetByIDAsync(id);

                if (booking == null)
                {
                    _logger.LogWarning("No Booking found with ID: {Id}", id);
                    return null;
                }

                // Only allow access if the user is the owner or an Admin
                if (booking.UserID != userId && role != "Admin")
                {
                    _logger.LogWarning("Access denied for user {UserId} with role {Role} to booking ID {Id}", userId, role, id);
                    return null;
                }

                _logger.LogInformation("Booking retrieved successfully. ID: {Id}", id);

                return booking;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Booking with ID: {Id}", id);
                return null;
            }
        }

    }
}
