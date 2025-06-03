using _01.FlightBookingSystem.Core.DTO_s.Booking;
using _01.FlightBookingSystem.Core.Models.Booking;

namespace _01.FlightBookingSystem.Core.Services
{
    /// <summary>
    /// Defines the contract for booking-related business logic services.
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Books a seat for a user.
        /// </summary>
        /// <param name="UserID">The identifier of the user making the booking.</param>
        /// <param name="SeatID">The identifier of the seat to book.</param>
        /// <returns>A <see cref="BookingResultDTO"/> indicating the success or failure of the booking operation.</returns>
        Task<BookingResultDTO> BookSeatAsync(string UserID, int SeatID);

        /// <summary>
        /// Cancels an existing booking based on the user identity and role.
        /// </summary>
        /// <param name="userId">The ID of the user attempting to cancel the booking.</param>
        /// <param name="role">The role of the user (e.g., Admin, User) to determine cancellation permissions.</param>
        /// <param name="bookingId">The ID of the booking to be canceled.</param>
        /// <returns>A <see cref="BookingResultDTO"/> indicating the result of the cancellation attempt.</returns>
        Task<BookingResultDTO> CancelBookingAsync(string userId, string role, int bookingId);


        /// <summary>
        /// Retrieves all bookings.
        /// </summary>
        /// <returns>A read-only list of all <see cref="Booking"/> records.</returns>
        Task<IReadOnlyList<Booking>> GetAllBookingsAsync();

        /// <summary>
        /// Confirms the payment of a booking by confirming a Stripe PaymentIntent.
        /// </summary>
        /// <param name="paymentIntentId">The Stripe PaymentIntent identifier to confirm.</param>
        /// <param name="PaymentMethodId">The Stripe PaymentMethod identifier to use for confirming the payment.</param>
        /// <returns>A <see cref="BookingResultDTO"/> indicating the success or failure of the payment confirmation.</returns>
        Task<BookingResultDTO> ConfirmBookingPaymentAsync(string paymentIntentId, string PaymentMethodId);

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
        Task<Booking> GetBookingByIDAsync(int id, string role, string userId);
    }
}
