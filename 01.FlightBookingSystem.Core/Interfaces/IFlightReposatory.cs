using _01.FlightBookingSystem.Core.Models.Flight;

namespace _01.FlightBookingSystem.Core.Interfaces
{
    /// <summary>
    /// Interface for Flight repository, extends generic repository interface for Flight entity.
    /// </summary>
    public interface IFlightReposatory : IGenericReposatory<Flight>
    {
        /// <summary>
        /// Retrieves a read-only list of upcoming flights including their associated seats.
        /// </summary>
        /// <returns>A list of upcoming Flight entities with their seats loaded.</returns>
        Task<IReadOnlyList<Flight>> GetUpcomingFlightsAndSeatsAsync();
    }
}
