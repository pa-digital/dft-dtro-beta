namespace DfT.DTRO.DAL;

/// <summary>
/// Service interface providing methods that support storage for D-TROs.
/// </summary>
public interface ITraDal
{
    /// <summary>
    /// Gets TRA records.
    /// </summary>
    /// <param name="parameters">Parameters passed</param>
    /// <returns>All active TRA records</returns>
    Task<IEnumerable<TrafficRegulationAuthority>> GetTrasAsync(GetAllTrasQueryParameters parameters);

    Task<TrafficRegulationAuthority> CreateTra();

    Task<TrafficRegulationAuthority> GetTraBySwaCode(int swaCode);
}