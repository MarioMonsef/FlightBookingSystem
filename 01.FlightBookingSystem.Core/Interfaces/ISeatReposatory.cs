using _01.FlightBookingSystem.Core.Models.Seat;

namespace _01.FlightBookingSystem.Core.Interfaces
{
    /// <summary>
    /// Interface for seat-specific repository operations.
    /// Extends the generic repository with flight-related seat queries.
    /// </summary>
    public interface ISeatReposatory : IGenericReposatory<Seat>
    {
        /// <summary>
        /// Retrieves a read-only list of available (not booked) seats for a specific flight.
        /// </summary>
        /// <param name="FlightID">The ID of the flight to retrieve available seats for.</param>
        /// <returns>A read-only list of seats that are available for booking.</returns>
        Task<IReadOnlyList<Seat>> GetAvailableSeats(int FlightID);

        /// <summary>
        /// Retrieves a read-only list of all seats (booked and available) for a specific flight.
        /// </summary>
        /// <param name="FlightID">The ID of the flight to retrieve all seats for.</param>
        /// <returns>A read-only list of all seats associated with the flight.</returns>
        Task<IReadOnlyList<Seat>> GetAllSeats(int FlightID);
    }
}
