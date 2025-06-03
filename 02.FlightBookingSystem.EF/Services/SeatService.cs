using _01.FlightBookingSystem.Core.Interfaces;
using _01.FlightBookingSystem.Core.Models.Seat;
using _01.FlightBookingSystem.Core.Services;
using Microsoft.Extensions.Logging;

namespace _02.FlightBookingSystem.EF.Services
{
    /// <summary>
    /// Service class responsible for handling Seat-related business logic.
    /// </summary>
    public class SeatService : BaseService, ISeatService
    {
        private readonly ILogger<SeatService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeatService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance.</param>
        /// <param name="logger">The logger instance for this service.</param>
        public SeatService(IUnitOfWork unitOfWork, ILogger<SeatService> logger) : base(unitOfWork)
        {
            _logger = logger;
        }

        /// <summary>
        /// Adds a new seat to the system.
        /// </summary>
        /// <param name="seat">The seat entity to add.</param>
        /// <returns>True if added successfully, otherwise false.</returns>
        public async Task<bool> AddSeatAsync(Seat seat)
        {
            if (seat == null)
            {
                _logger.LogWarning("AddSeatAsync called with null seat.");
                return false;
            }

            try
            {
                await _unitOfWork.SeatReposatory.AddAsync(seat);
                await _unitOfWork.Complete();
                _logger.LogInformation("Seat added successfully. Seat ID: {SeatId}", seat.ID);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding seat.");
                return false;
            }
        }

        /// <summary>
        /// Gets a seat by its ID including its flight details.
        /// </summary>
        /// <param name="id">The seat ID.</param>
        /// <returns>The seat entity or null if not found.</returns>
        public async Task<Seat> GetSeatByIDAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("GetSeatByIDAsync called with invalid ID: {Id}", id);
                return null;
            }

            try
            {
                var seat = await _unitOfWork.SeatReposatory.GetByIDAsync(id, s => s.Flight);
                if (seat == null)
                    _logger.LogWarning("No seat found with ID: {Id}", id);
                return seat;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting seat by ID: {Id}", id);
                return null;
            }
        }

        /// <summary>
        /// Removes a seat by ID.
        /// </summary>
        /// <param name="id">The seat ID.</param>
        /// <returns>True if removed successfully, otherwise false.</returns>
        public async Task<bool> RemoveSeatByIDAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("RemoveSeatByIDAsync called with invalid ID: {Id}", id);
                return false;
            }

            try
            {
                await _unitOfWork.SeatReposatory.DeleteAsync(id);
                await _unitOfWork.Complete();
                _logger.LogInformation("Seat removed successfully. Seat ID: {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing seat with ID: {Id}", id);
                return false;
            }
        }

        /// <summary>
        /// Updates a seat entity.
        /// </summary>
        /// <param name="seat">The seat entity with updated data.</param>
        /// <returns>True if updated successfully, otherwise false.</returns>
        public async Task<bool> UpdateSeatAsync(Seat seat)
        {
            if (seat == null)
            {
                _logger.LogWarning("UpdateSeatAsync called with null seat.");
                return false;
            }

            try
            {
                _unitOfWork.SeatReposatory.Update(seat);
                await _unitOfWork.Complete();
                _logger.LogInformation("Seat updated successfully. Seat ID: {SeatId}", seat.ID);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating seat with ID: {SeatId}", seat.ID);
                return false;
            }
        }

        /// <summary>
        /// Gets available seats for a flight by flight ID.
        /// </summary>
        /// <param name="flightId">The flight ID.</param>
        /// <returns>A read-only list of available seats.</returns>
        public async Task<IReadOnlyList<Seat>> GetAvailableSeats(int flightId)
        {
            if (flightId <= 0)
            {
                _logger.LogWarning("GetAvailableSeats called with invalid flightID: {FlightID}", flightId);
                return new List<Seat>();
            }

            try
            {
                return await _unitOfWork.SeatReposatory.GetAvailableSeats(flightId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available seats for FlightID: {FlightID}", flightId);
                return new List<Seat>();
            }
        }

        /// <summary>
        /// Gets all seats (available and booked) for a flight.
        /// </summary>
        /// <param name="flightId">The flight ID.</param>
        /// <returns>A read-only list of all seats for the specified flight.</returns>
        public async Task<IReadOnlyList<Seat>> GetAllSeats(int flightId)
        {
            if (flightId <= 0)
            {
                _logger.LogWarning("GetAllSeats called with invalid flightID: {FlightID}", flightId);
                return new List<Seat>();
            }

            try
            {
                return await _unitOfWork.SeatReposatory.GetAllSeats(flightId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all seats for FlightID: {FlightID}", flightId);
                return new List<Seat>();
            }
        }
    }
}
