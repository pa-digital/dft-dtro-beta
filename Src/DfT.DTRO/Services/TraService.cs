using DfT.DTRO.Models.Tra;

namespace DfT.DTRO.Services;

public class TraService : ITraService
{
    private readonly ITraDal _traDal;
    private readonly IDtroMappingService _dtroMappingService;

    public TraService(
        ITraDal traDal,
        IDtroMappingService dtroMappingService)
    {
        _traDal = traDal;
        _dtroMappingService = dtroMappingService;
    }
    
    public async Task<IEnumerable<TraFindAllResponse>> GetTrasAsync(GetAllTrasQueryParameters parameters)
    {
        List<TrafficRegulationAuthority> tras = (await _traDal.GetTrasAsync(parameters)).ToList();
        if (!tras.Any())
        {
            throw new NotFoundException();
        }
        return tras.Select(_dtroMappingService.MapToTraFindAllResponse);
    }
}