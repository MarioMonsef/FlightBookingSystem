using _01.FlightBookingSystem.Core.Models.Booking;

namespace _01.FlightBookingSystem.Core.Interfaces
{
    /// <summary>
    /// Defines repository operations specific to Booking entity,
    /// extends the generic repository interface.
    /// </summary>
    public interface IBookingReposatory : IGenericReposatory<Booking>
    {
        /// <summary>
        /// Retrieves a booking based on the Stripe payment intent ID.
        /// </summary>
        /// <param name="StripePaymentIntentId">The Stripe payment intent identifier.</param>
        /// <returns>The booking entity matching the payment intent ID, or null if not found.</returns>
        Task<Booking> GetByPaymentIntintID(string StripePaymentIntentId);
    }
}
