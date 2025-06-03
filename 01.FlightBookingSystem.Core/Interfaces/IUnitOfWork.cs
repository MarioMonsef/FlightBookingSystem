using Microsoft.EntityFrameworkCore.Storage;

namespace _01.FlightBookingSystem.Core.Interfaces
{
    /// <summary>
    /// Represents a unit of work for managing repositories and database transactions.
    /// Provides a single point to commit changes and manage multiple repositories.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the booking repository instance.
        /// </summary>
        IBookingReposatory BookingReposatory { get; }

        /// <summary>
        /// Gets the flight repository instance.
        /// </summary>
        IFlightReposatory FlightReposatory { get; }

        /// <summary>
        /// Gets the seat repository instance.
        /// </summary>
        ISeatReposatory SeatReposatory { get; }

        /// <summary>
        /// Saves all changes made in this unit of work to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> Complete();

        /// <summary>
        /// Begins a new database transaction asynchronously.
        /// </summary>
        /// <returns>The database context transaction.</returns>
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
