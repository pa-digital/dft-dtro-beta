namespace DfT.DTRO.Repositories.Contracts;

/// <summary>
/// Defines a generic repository interface for entities inheriting from <see cref="BaseEntity"/>
/// </summary>
/// <typeparam name="T"><see cref="Type"/> of the entity manage the repository</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Create a new entity to the repository asynchronously
    /// </summary>
    /// <param name="entity">The entity to create</param>
    /// <returns>A task representing the asynchronous operation, returning the created entity</returns>
    Task<T> CreateAsync(T entity);

    /// <summary>
    /// Retrieves all entities in the repository asynchronously
    /// </summary>
    /// <returns>A task representing the asynchronous operation, returning an enumerate collection of all entities.</returns>
    Task<IEnumerable<T>> ReadAsync();

    /// <summary>
    /// Retrieves an entity by its unique identifier asynchronously
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve</param>
    /// <returns>A task representing the asynchronous operation, returning the retrieved entity, or null if not found</returns>
    Task<T> ReadAsync(Guid id);

    /// <summary>
    /// Updates an existing entity in the repository asynchronously
    /// </summary>
    /// <param name="entity">The entity to update</param>
    /// <returns>A task returning the asynchronous operation, returning the updated entity</returns>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Delete an existing entity in the repository asynchronously
    /// </summary>
    /// <param name="entity">The entity to delete</param>
    /// <returns>A task returning the asynchronous operation, returning true if successfuly deleted</returns>
    Task<bool> DeleteAsync(T entity);
}