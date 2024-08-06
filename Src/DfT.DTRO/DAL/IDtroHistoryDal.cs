namespace DfT.DTRO.DAL;

/// <summary>
/// Service interface providing methods that support storage of D-TROs history.
/// </summary>
public interface IDtroHistoryDal
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dtroHistory"></param>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true" />
    /// if a D-TRO history has been saved. Otherwise <see langword="false" />
    /// </returns>
    Task<bool> SaveDtroInHistoryTable(DTROHistory dtroHistory);

    /// <summary>
    /// Get a D-TRO history by its <paramref name="dtroId"/>
    /// </summary>
    /// <param name="dtroId">DTRO linked ID</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous list of D-TROs.</returns>
    Task<List<DTROHistory>> GetDtroHistory(Guid dtroId);
}