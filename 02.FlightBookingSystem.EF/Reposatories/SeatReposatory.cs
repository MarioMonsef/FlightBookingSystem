using _01.FlightBookingSystem.Core.Interfaces;
using _01.FlightBookingSystem.Core.Models.Seat;
using _02.FlightBookingSystem.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace _02.FlightBookingSystem.EF.Reposatories
{
    /// <summary>
    /// Concrete repository implementation for handling Seat-specific queries.
    /// </summary>
    public class SeatReposatory : GenericReposatory<Seat>, ISeatReposatory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeatReposatory"/> class.
        /// </summary>
        /// <param name="_context">The application database context.</param>
        public SeatReposatory(ApplicationDbContext _context) : base(_context)
        {
        }

        /// <summary>
        /// Retrieves a read-only list of seats for a specific flight where the seat is not booked.
        /// </summary>
        /// <param name="FlightID">The flight ID to filter seats by.</param>
        /// <returns>A read-only list of available seats for the given flight.</returns>
        public async Task<IReadOnlyList<Seat>> GetAvailableSeats(int FlightID) =>
            await _context.Seats
                .Where(s => s.FlightID == FlightID && !s.IsBooking) 
                .Include(s => s.Flight) 
                .AsNoTracking() 
                .ToListAsync();

        /// <summary>
        /// Retrieves all seats (booked and available) for a specific flight.
        /// </summary>
        /// <param name="FlightID">The flight ID to filter seats by.</param>
        /// <returns>A read-only list of all seats for the given flight.</returns>
        public async Task<IReadOnlyList<Seat>> GetAllSeats(int FlightID) =>
            await _context.Seats
                .Where(s => s.FlightID == FlightID)
                .Include(s => s.Flight)
                .AsNoTracking()
                .ToListAsync();
    }
}
