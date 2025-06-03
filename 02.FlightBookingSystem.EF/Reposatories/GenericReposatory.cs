using _01.FlightBookingSystem.Core.Interfaces;
using _02.FlightBookingSystem.EF.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace _02.FlightBookingSystem.EF.Reposatories
{
    /// <summary>
    /// Generic repository implementation for basic CRUD operations.
    /// </summary>
    /// <typeparam name="T">The entity type (must be a class).</typeparam>
    public class GenericReposatory<T> : IGenericReposatory<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public GenericReposatory(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds an entity asynchronously to the context.
        /// </summary>
        /// <param name="item">The entity to add.</param>
        public async Task AddAsync(T item)
            => await _context.Set<T>().AddAsync(item);

        /// <summary>
        /// Gets the total count of entities in the table.
        /// </summary>
        /// <returns>Total number of records.</returns>
        public async Task<int> Count()
            => await _context.Set<T>().CountAsync();

        /// <summary>
        /// Deletes an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        public async Task DeleteAsync(int id)
        {
            // Find entity by ID using EF's FindAsync
            var item = await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(item);
        }

        /// <summary>
        /// Retrieves all entities of type T with no tracking.
        /// </summary>
        /// <returns>List of all entities.</returns>
        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await _context.Set<T>().AsNoTracking().ToListAsync();

        /// <summary>
        /// Retrieves all entities of type T with related entities included.
        /// </summary>
        /// <param name="includes">Navigation properties to include.</param>
        /// <returns>List of all entities with includes.</returns>
        public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsNoTracking().AsQueryable();

            // Apply all includes for eager loading
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Retrieves an entity by ID with specified navigation properties included.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="includes">Navigation properties to include.</param>
        /// <returns>The matched entity or throws if not found.</returns>
        public async Task<T> GetByIDAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();

            // Apply includes
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Use reflection to compare entity ID property
            var entity = await query.FirstAsync(x => Microsoft.EntityFrameworkCore.EF.Property<int>(x, "ID") == id);
            return entity;
        }

        /// <summary>
        /// Retrieves an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The matched entity or null if not found.</returns>
        public async Task<T> GetByIDAsync(int id)
            => await _context.Set<T>().FindAsync(id);

        /// <summary>
        /// Updates an entity's state to Modified.
        /// </summary>
        /// <param name="item">The entity to update.</param>
        public void Update(T item)
            => _context.Entry(item).State = EntityState.Modified;
    }
}
