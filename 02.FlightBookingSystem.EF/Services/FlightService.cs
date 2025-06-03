using _01.FlightBookingSystem.Core.Interfaces;
using _01.FlightBookingSystem.Core.Models.Flight;
using _01.FlightBookingSystem.Core.Services;
using Microsoft.Extensions.Logging;

namespace _02.FlightBookingSystem.EF.Services
{
    public class FlightService : BaseService, IFlightService
    {
        private readonly ILogger<FlightService> _logger;

        public FlightService(IUnitOfWork unitOfWork, ILogger<FlightService> logger)
            : base(unitOfWork)
        {
            _logger = logger;
        }

        /// <summary>
        /// Adds a new flight to the system.
        /// </summary>
        /// <param name="flight">The flight entity to be added.</param>
        /// <returns>True if the flight was added successfully; otherwise, false.</returns>
        public async Task<bool> AddFlight(Flight flight)
        {
            if (flight == null)
            {
                _logger.LogWarning("Attempted to add a null flight.");
                return false;
            }

            try
            {
                await _unitOfWork.FlightReposatory.AddAsync(flight);
                await _unitOfWork.Complete();
                _logger.LogInformation("Flight added successfully. Flight ID: {FlightId}", flight.ID);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a flight.");
                return false;
            }
        }

        /// <summary>
        /// Retrieves all flights from the database including their related seats.
        /// </summary>
        /// <returns>A read-only list of all flights with their seat information.</returns>
        public async Task<IReadOnlyList<Flight>> GetAllFlights()
        {
            _logger.LogInformation("Fetching all flights with seats.");
            try
            {
                var flights = await _unitOfWork.FlightReposatory.GetAllAsync(f => f.Seats);
                _logger.LogInformation("Retrieved {Count} flights.", flights.Count);
                return flights;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all flights.");
                return new List<Flight>();
            }
        }

        /// <summary>
        /// Retrieves a flight by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the flight to retrieve.</param>
        /// <returns>The flight with the specified ID, or null if not found or if an error occurs.</returns>
        public async Task<Flight> GetFlightByID(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid flight ID received: {Id}", id);
                return null;
            }

            try
            {
                var flight = await _unitOfWork.FlightReposatory.GetByIDAsync(id, f => f.Seats);
                if (flight == null)
                    _logger.LogWarning("No flight found with ID: {Id}", id);
                else
                    _logger.LogInformation("Flight retrieved. ID: {Id}", id);

                return flight;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving flight with ID: {Id}", id);
                return null;
            }
        }

        /// <summary>
        /// Removes a flight from the system by its ID.
        /// </summary>
        /// <param name="flightID">The ID of the flight to remove.</param>
        /// <returns>True if the flight was removed successfully; otherwise, false.</returns>
        public async Task<bool> RemoveFlight(int flightID)
        {
            if (flightID <= 0)
            {
                _logger.LogWarning("Invalid flight ID received for removal: {FlightID}", flightID);
                return false;
            }

            try
            {
                await _unitOfWork.FlightReposatory.DeleteAsync(flightID);
                await _unitOfWork.Complete();
                _logger.LogInformation("Flight removed successfully. ID: {FlightID}", flightID);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing flight. ID: {FlightID}", flightID);
                return false;
            }
        }

        /// <summary>
        /// Updates an existing flight in the system.
        /// </summary>
        /// <param name="flight">The flight entity with updated information.</param>
        /// <returns>True if the flight was updated successfully; otherwise, false.</returns>
        public async Task<bool> UpdateFlight(Flight flight)
        {
            if (flight == null)
            {
                _logger.LogWarning("Attempted to update a null flight.");
                return false;
            }

            try
            {
                _unitOfWork.FlightReposatory.Update(flight);
                await _unitOfWork.Complete();
                _logger.LogInformation("Flight updated successfully. ID: {FlightID}", flight.ID);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating flight. ID: {FlightID}", flight.ID);
                return false;
            }
        }

        /// <summary>
        /// Retrieves a list of upcoming flights that haven't departed yet.
        /// </summary>
        /// <returns>A read-only list of upcoming flights.</returns>
        public async Task<IReadOnlyList<Flight>> GetUpcomingFlightsAsync()
        {
            try
            {
                var upcomingFlights = await _unitOfWork.FlightReposatory.GetUpcomingFlightsAndSeatsAsync();
                    
                _logger.LogInformation("Retrieved {Count} upcoming flights.", upcomingFlights.Count);
                return upcomingFlights;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching upcoming flights.");
                return new List<Flight>();
            }
        }

    }
}
