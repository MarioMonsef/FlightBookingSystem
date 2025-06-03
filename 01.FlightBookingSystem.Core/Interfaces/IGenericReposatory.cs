using System.Linq.Expressions;

namespace _01.FlightBookingSystem.Core.Interfaces
{
    /// <summary>
    /// Generic repository interface for performing basic CRUD operations.
    /// </summary>
    /// <typeparam name="T">The entity type (must be a class).</typeparam>
    public interface IGenericReposatory<T> where T : class
    {
        /// <summary>
        /// Gets an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The entity matching the ID or null if not found.</returns>
        Task<T> GetByIDAsync(int id);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>A read-only list of all entities.</returns>
        Task<IReadOnlyList<T>> GetAllAsync();

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="item">The entity to update.</param>
        void Update(T item);

        /// <summary>
        /// Adds a new entity asynchronously.
        /// </summary>
        /// <param name="item">The entity to add.</param>
        Task AddAsync(T item);

        /// <summary>
        /// Gets the total number of entities.
        /// </summary>
        /// <returns>The count of entities.</returns>
        Task<int> Count();

        /// <summary>
        /// Gets all entities with specified navigation properties included.
        /// </summary>
        /// <param name="includes">The navigation properties to include.</param>
        /// <returns>A read-only list of all entities with includes.</returns>
        Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Gets an entity by its ID and includes specified navigation properties.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <param name="includes">The navigation properties to include.</param>
        /// <returns>The entity matching the ID with included properties.</returns>
        Task<T> GetByIDAsync(int id, params Expression<Func<T, object>>[] includes);
    }
}
