using _01.FlightBookingSystem.Core.Models.Seat;

namespace _01.FlightBookingSystem.Core.Services
{
    /// <summary>
    /// Defines contract for seat-related operations in the flight booking system.
    /// </summary>
    public interface ISeatService
    {
        /// <summary>
        /// Adds a new seat to the system.
        /// </summary>
        /// <param name="seat">The seat object to be added.</param>
        /// <returns>True if the seat is added successfully; otherwise, false.</returns>
        Task<bool> AddSeatAsync(Seat seat);

        /// <summary>
        /// Retrieves a seat by its unique identifier.
        /// </summary>
        /// <param name="ID">The unique identifier of the seat.</param>
        /// <returns>The seat with the specified ID, or null if not found.</returns>
        Task<Seat> GetSeatByIDAsync(int ID);

        /// <summary>
        /// Removes a seat from the system by its unique identifier.
        /// </summary>
        /// <param name="ID">The unique identifier of the seat to be removed.</param>
        /// <returns>True if the seat was successfully removed; otherwise, false.</returns>
        Task<bool> RemoveSeatByIDAsync(int ID);

        /// <summary>
        /// Updates the details of an existing seat.
        /// </summary>
        /// <param name="seat">The seat object containing updated information.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        Task<bool> UpdateSeatAsync(Seat seat);

        /// <summary>
        /// Retrieves a list of available seats for a specific flight.
        /// </summary>
        /// <param name="FlightID">The unique identifier of the flight.</param>
        /// <returns>A read-only list of available seats for the specified flight.</returns>
        Task<IReadOnlyList<Seat>> GetAvailableSeats(int FlightID);
        /// <summary>
        /// Gets all seats (available and booked) for a flight.
        /// </summary>
        /// <param name="flightId">The flight ID.</param>
        /// <returns>A read-only list of all seats for the specified flight.</returns>
        Task<IReadOnlyList<Seat>> GetAllSeats(int flightId);
    }
}
