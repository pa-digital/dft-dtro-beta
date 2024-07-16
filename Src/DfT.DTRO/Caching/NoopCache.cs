using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace DfT.DTRO.Caching;

[ExcludeFromCodeCoverage]
public class NoopCache : IRedisCache
{
    public Task CacheDtro(Models.DataBase.DTRO dtro)
        => Task.CompletedTask;

    public Task CacheDtroExists(Guid key, bool value)
        => Task.CompletedTask;

    public Task<Models.DataBase.DTRO> GetDtro(Guid key)
        => Task.FromResult<Models.DataBase.DTRO>(null);

    public Task<bool?> GetDtroExists(Guid key)
        => Task.FromResult<bool?>(null);

    public Task RemoveDtro(Guid key)
        => Task.CompletedTask;

    public Task RemoveDtroIfExists(Guid key)
        => Task.CompletedTask;
}
