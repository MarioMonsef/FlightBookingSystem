using _01.FlightBookingSystem.Core.Interfaces;
using _01.FlightBookingSystem.Core.Models.Booking;
using _02.FlightBookingSystem.EF.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace _02.FlightBookingSystem.EF.Reposatories
{
    /// <summary>
    /// Repository implementation for Booking entity, extends the generic repository.
    /// </summary>
    public class BookingReposatory : GenericReposatory<Booking>, IBookingReposatory
    {
        /// <summary>
        /// Constructor injecting the database context.
        /// </summary>
        /// <param name="_context">The application database context.</param>
        public BookingReposatory(ApplicationDbContext _context) : base(_context)
        {
        }

        /// <summary>
        /// Retrieves a booking entity based on the Stripe payment intent ID.
        /// </summary>
        /// <param name="StripePaymentIntentId">The Stripe payment intent identifier.</param>
        /// <returns>The booking entity if found; otherwise null.</returns>
        public async Task<Booking> GetByPaymentIntintID(string StripePaymentIntentId) =>
            await _context.Bookings.FirstOrDefaultAsync(b => b.StripePaymentIntentId == StripePaymentIntentId);
    }
}
