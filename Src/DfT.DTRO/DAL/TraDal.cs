namespace DfT.DTRO.DAL;

/// <summary>
/// Implementation of the <see cref="IDtroDal" /> service.
/// </summary>
[ExcludeFromCodeCoverage]
public class TraDal : ITraDal
{
    private readonly DtroContext _dtroContext;
    private readonly IDtroMappingService _dtroMappingService;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="dtroContext"><see cref="DtroContext"/> database context.</param>
    /// <param name="dtroMappingService"><see cref="IDtroMappingService"/> service.</param>
    public TraDal(DtroContext dtroContext, IDtroMappingService dtroMappingService)
    {
        _dtroContext = dtroContext;
        _dtroMappingService = dtroMappingService;
    }

    ///<inheritdoc cref="ITraDal"/>
    public async Task<IEnumerable<TrafficRegulationAuthority>> GetTrasAsync(GetAllTrasQueryParameters parameters)
    {
        IQueryable<TrafficRegulationAuthority> trasQuery = _dtroContext.TrafficRegulationAuthorities;
        trasQuery = parameters.TraName != null ? trasQuery.Where(tra => tra.Name.ToLower().Contains(parameters.TraName.ToLower())) : trasQuery;
        var tras = await trasQuery.ToListAsync();
        return tras;
    }

    public async Task<TrafficRegulationAuthority> CreateTra()
    {
        Random random = new Random();
        bool isUnique = false;
        int dummySwaCode;
        do
        {
            dummySwaCode = random.Next(1, 10000);
            isUnique = !await _dtroContext.TrafficRegulationAuthorities.AnyAsync(e => e.SwaCode == dummySwaCode);
        }
        while (!isUnique);

        TrafficRegulationAuthority tra = new TrafficRegulationAuthority { Name = "New TRA", Status = "Active", SwaCode = dummySwaCode };
        _dtroContext.TrafficRegulationAuthorities.Add(tra);
        await _dtroContext.SaveChangesAsync();
        return tra;
    }

    public async Task<TrafficRegulationAuthority> GetTraBySwaCode(int swaCode)
    {
        return await _dtroContext.TrafficRegulationAuthorities.SingleOrDefaultAsync(tra => tra.SwaCode == swaCode);
    }
}