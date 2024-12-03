namespace DfT.DTRO.Caching;

/// <summary>
/// Service interface providing methods that support caching D-TRO extraction API response data.
/// </summary>
public interface IRedisCache
{
    /// <summary>
    /// Add D-TRO records to cache.
    /// </summary>
    /// <param name="dtros">List of D-TRO records to cache.</param>
    /// <returns>A <see cref="Task"/> represending the asynchronous save operation.</returns>
    Task CacheDtros(List<Models.DataBase.DTRO> dtros);

    /// <summary>
    /// Service method that will get existing D-TROs.
    /// </summary>
    /// <returns>A list of active D-TRO</returns>
    Task<List<Models.DataBase.DTRO>> GetDtros();

    /// <summary>
    /// Service method that will get existing D-TROs by its unique identifier.
    /// </summary>
    /// <param name="key">D-TRO ID.</param>
    /// <returns>A D-TRO</returns>
    Task<Models.DataBase.DTRO> GetDtro(Guid key);

    /// <summary>
    /// Add D-TRO to cache.
    /// </summary>
    /// <param name="dtro">D-TRO object to cache.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    Task CacheDtro(Models.DataBase.DTRO dtro);

    /// <summary>
    /// Check if the D-TRO exists.
    /// </summary>
    /// <param name="key">D-TRO ID.</param>
    /// <returns>Flag indicating if the D-TRO exists.</returns>
    Task<bool?> GetDtroExists(Guid key);

    /// <summary>
    /// Check if the D-TRO exists in the cache.
    /// </summary>
    /// <param name="key">D-TRO ID.</param>
    /// <param name="value">Flag to set.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    Task CacheDtroExists(Guid key, bool value);

    /// <summary>
    /// Remove D-TRO from cache.
    /// </summary>
    /// <param name="key">D-TRO ID</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    Task RemoveDtro(Guid key);

    /// <summary>
    /// Remove D-TRO from cache if exists.
    /// </summary>
    /// <param name="key">D-TRO ID.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    Task RemoveDtroIfExists(Guid key);
}
