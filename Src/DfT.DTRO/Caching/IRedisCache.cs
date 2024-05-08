using System;
using System.Threading.Tasks;

namespace DfT.DTRO.Caching;

/// <summary>
/// Provides methods that support caching DTRO extraction API response data.
/// </summary>
public interface IRedisCache
{
    Task<Models.DataBase.DTRO> GetDtro(Guid key);

    Task CacheDtro(Models.DataBase.DTRO dtro);

    Task<bool?> GetDtroExists(Guid key);

    Task CacheDtroExists(Guid key, bool value);

    Task RemoveDtro(Guid key);

    Task RemoveDtroIfExists(Guid key);
}
