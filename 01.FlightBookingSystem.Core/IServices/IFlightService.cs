using _01.FlightBookingSystem.Core.Models.Flight;

namespace _01.FlightBookingSystem.Core.Services
{
    /// <summary>
    /// Defines the contract for flight-related operations.
    /// </summary>
    public interface IFlightService
    {
        /// <summary>
        /// Retrieves all flights including their related seats.
        /// </summary>
        /// <returns>A read-only list of all flights.</returns>
        Task<IReadOnlyList<Flight>> GetAllFlights();

        /// <summary>
        /// Adds a new flight to the system.
        /// </summary>
        /// <param name="flight">The flight entity to add.</param>
        /// <returns>True if the flight was added successfully; otherwise, false.</returns>
        Task<bool> AddFlight(Flight flight);

        /// <summary>
        /// Removes a flight by its unique identifier.
        /// </summary>
        /// <param name="FlightID">The ID of the flight to remove.</param>
        /// <returns>True if the flight was removed successfully; otherwise, false.</returns>
        Task<bool> RemoveFlight(int FlightID);

        /// <summary>
        /// Updates an existing flight.
        /// </summary>
        /// <param name="flight">The flight entity with updated information.</param>
        /// <returns>True if the flight was updated successfully; otherwise, false.</returns>
        Task<bool> UpdateFlight(Flight flight);

        /// <summary>
        /// Retrieves a flight by its unique identifier.
        /// </summary>
        /// <param name="ID">The ID of the flight to retrieve.</param>
        /// <returns>The flight with the specified ID, or null if not found.</returns>
        Task<Flight> GetFlightByID(int ID);

        /// <summary>
        /// Retrieves a list of upcoming flights that haven't departed yet.
        /// </summary>
        /// <returns>A read-only list of upcoming flights.</returns>
        Task<IReadOnlyList<Flight>> GetUpcomingFlightsAsync();

    }
}
