using _01.FlightBookingSystem.Core.Interfaces;
using _01.FlightBookingSystem.Core.Models.Flight;
using _02.FlightBookingSystem.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace _02.FlightBookingSystem.EF.Reposatories
{
    /// <summary>
    /// Repository class for managing Flight entities.
    /// Inherits from GenericRepository and implements IFlightRepository interface.
    /// </summary>
    public class FlightReposatory : GenericReposatory<Flight>, IFlightReposatory
    {
        /// <summary>
        /// Constructor that passes the database context to the base repository class.
        /// </summary>
        /// <param name="_context">The ApplicationDbContext instance.</param>
        public FlightReposatory(ApplicationDbContext _context) : base(_context)
        {
        }

        /// <summary>
        /// Retrieves a list of upcoming flights (departure time in the future) including their associated seats.
        /// </summary>
        /// <returns>A read-only list of upcoming Flight entities with their Seats included.</returns>
        public async Task<IReadOnlyList<Flight>> GetUpcomingFlightsAndSeatsAsync() =>
            await _context.flights
                .Where(f => f.DepartureTime > DateTime.UtcNow)
                .Include(f => f.Seats) 
                .ToListAsync();
    }
}
