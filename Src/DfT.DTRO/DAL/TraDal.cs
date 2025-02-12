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
}