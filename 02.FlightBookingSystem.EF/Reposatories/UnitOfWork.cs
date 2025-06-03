using _01.FlightBookingSystem.Core.Interfaces;
using _02.FlightBookingSystem.EF.Entities;
using _02.FlightBookingSystem.EF.Reposatories;
using Microsoft.EntityFrameworkCore.Storage;

namespace _02.FlightBookingSystem.EF.Repositories
{
    /// <summary>
    /// Handles coordination of repository interactions and database transactions.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private IBookingReposatory _bookingReposatory;
        private IFlightReposatory _flightReposatory;
        private ISeatReposatory _seatReposatory;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the booking repository instance.
        /// </summary>
        public IBookingReposatory BookingReposatory =>
            _bookingReposatory ??= new BookingReposatory(_context);

        /// <summary>
        /// Gets the flight repository instance.
        /// </summary>
        public IFlightReposatory FlightReposatory =>
            _flightReposatory ??= new FlightReposatory(_context);

        /// <summary>
        /// Gets the seat repository instance.
        /// </summary>
        public ISeatReposatory SeatReposatory =>
            _seatReposatory ??= new SeatReposatory(_context);

        /// <summary>
        /// Begins a new database transaction asynchronously.
        /// </summary>
        /// <returns>An <see cref="IDbContextTransaction"/> instance representing the transaction.</returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
            => await _context.Database.BeginTransactionAsync();

        /// <summary>
        /// Saves all changes made in this context to the database asynchronously.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public async Task<int> Complete()
            => await _context.SaveChangesAsync();

        /// <summary>
        /// Disposes the database context.
        /// </summary>
        public void Dispose()
            => _context.Dispose();
    }
}
