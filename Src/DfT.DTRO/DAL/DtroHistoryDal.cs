namespace DfT.DTRO.DAL;

/// <summary>
/// Implementation of the <see cref="IDtroHistoryDal"/> service.
/// </summary>
public class DtroHistoryDal : IDtroHistoryDal
{
    private readonly DtroContext _dtroContext;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="dtroContext"><see cref="DtroContext"/> database context.</param>
    public DtroHistoryDal(DtroContext dtroContext)
    {
        _dtroContext = dtroContext;
    }

    ///<inheritdoc cref="IDtroHistoryDal" />
    public async Task<bool> SaveDtroInHistoryTable(DTROHistory dtroHistory)
    {
        EntityEntry<DTROHistory> entry = await _dtroContext.DtroHistories.AddAsync(dtroHistory);
        if (entry.Entity.Id == Guid.Empty)
        {
            return false;
        }

        await _dtroContext.SaveChangesAsync();
        return true;
    }

    ///<inheritdoc cref="IDtroHistoryDal" />
    public async Task<List<DTROHistory>> GetDtroHistory(Guid dtroId)
    {
        try
        {
            var result = await _dtroContext.DtroHistories
                .Where(history => history.DtroId == dtroId)
                .OrderByDescending(history => history.LastUpdated)
                .ToListAsync();

            return result;
        }
        catch (Exception ex) {
            throw new Exception($"Error: Unable to get retrieving history for '{dtroId}'", ex);
        }
    }
}