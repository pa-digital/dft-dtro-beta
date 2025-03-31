using DfT.DTRO.Models.Tra;

namespace DfT.DTRO.Services;

public interface ITraService
{

    /// <summary>
    /// Get all TRA records withing parameters
    /// </summary>
    /// <param name="parameters">Parameters passed</param>
    /// <returns>List of D-TRO records</returns>
    Task<IEnumerable<TraFindAllResponse>> GetTrasAsync(GetAllTrasQueryParameters parameters);

}