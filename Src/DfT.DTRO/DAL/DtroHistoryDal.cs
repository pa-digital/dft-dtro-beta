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
    public async Task<bool> SaveDtroInHistoryTable(DigitalTrafficRegulationOrderHistory digitalTrafficRegulationOrderHistory)
    {
        EntityEntry<DigitalTrafficRegulationOrderHistory> entry = await _dtroContext.DigitalTrafficRegulationOrderHistories.AddAsync(digitalTrafficRegulationOrderHistory);
        if (entry.Entity.Id == Guid.Empty)
        {
            return false;
        }

        await _dtroContext.SaveChangesAsync();
        return true;
    }

    ///<inheritdoc cref="IDtroHistoryDal" />
    public async Task<List<DigitalTrafficRegulationOrderHistory>> GetDtroHistory(Guid dtroId)
    {
        try
        {
            var result = await _dtroContext.DigitalTrafficRegulationOrderHistories
                .Where(history => history.DigitalTrafficRegulationOrderId == dtroId)
                .OrderByDescending(history => history.LastUpdated)
                .ToListAsync();

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: Unable to get retrieving history for '{dtroId}'", ex);
        }
    }
}