namespace DfT.DTRO.Caching;

/// <summary>
/// Class used for unit testing.
/// </summary>
[ExcludeFromCodeCoverage]
public class NoopCache : IRedisCache
{
    /// <inheritdoc cref="IRedisCache"/>
    public Task CacheDtro(Models.DataBase.DigitalTrafficRegulationOrder digitalTrafficRegulationOrder)
        => Task.CompletedTask;

    /// <inheritdoc cref="IRedisCache"/>
    public Task CacheDtroExists(Guid key, bool value)
        => Task.CompletedTask;

    /// <inheritdoc cref="IRedisCache"/>
    public Task<List<Models.DataBase.DigitalTrafficRegulationOrder>> CacheDtros(IEnumerable<Models.DataBase.DigitalTrafficRegulationOrder> dtros) =>
        Task.CompletedTask as Task<List<Models.DataBase.DigitalTrafficRegulationOrder>>;

    /// <inheritdoc cref="IRedisCache"/>
    public Task CacheDtros(List<Models.DataBase.DigitalTrafficRegulationOrder> dtros) =>
        Task.CompletedTask;

    /// <inheritdoc cref="IRedisCache"/>
    public Task<List<Models.DataBase.DigitalTrafficRegulationOrder>> GetDtros() =>
        Task.FromResult(new List<Models.DataBase.DigitalTrafficRegulationOrder>());

    /// <inheritdoc cref="IRedisCache"/>
    public Task<Models.DataBase.DigitalTrafficRegulationOrder> GetDtro(Guid key)
        => Task.FromResult<Models.DataBase.DigitalTrafficRegulationOrder>(null);

    /// <inheritdoc cref="IRedisCache"/>
    public Task<bool?> GetDtroExists(Guid key)
        => Task.FromResult<bool?>(null);

    /// <inheritdoc cref="IRedisCache"/>
    public Task RemoveDtro(Guid key)
        => Task.CompletedTask;

    /// <inheritdoc cref="IRedisCache"/>
    public Task RemoveDtroIfExists(Guid key)
        => Task.CompletedTask;
}
