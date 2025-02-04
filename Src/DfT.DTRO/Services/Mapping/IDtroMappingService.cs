namespace DfT.DTRO.Services.Mapping;

public interface IDtroMappingService
{
    void InferIndexFields(ref Models.DataBase.DigitalTrafficRegulationOrder digitalTrafficRegulationOrder);

    DtroResponse MapToDtroResponse(Models.DataBase.DigitalTrafficRegulationOrder digitalTrafficRegulationOrder);

    IEnumerable<DtroEvent> MapToEvents(IEnumerable<Models.DataBase.DigitalTrafficRegulationOrder> dtros, DateTime? searchSince);

    IEnumerable<DtroSearchResult> MapToSearchResult(IEnumerable<Models.DataBase.DigitalTrafficRegulationOrder> dtros);

    DtroHistorySourceResponse GetSource(DigitalTrafficRegulationOrderHistory digitalTrafficRegulationOrderHistory);

    List<DtroHistoryProvisionResponse> GetProvision(DigitalTrafficRegulationOrderHistory digitalTrafficRegulationOrderHistory);

    DigitalTrafficRegulationOrderHistory MapToDtroHistory(Models.DataBase.DigitalTrafficRegulationOrder currentDigitalTrafficRegulationOrder);

    DtroOwner GetOwnership(Models.DataBase.DigitalTrafficRegulationOrder digitalTrafficRegulationOrder);

    void SetOwnership(ref Models.DataBase.DigitalTrafficRegulationOrder digitalTrafficRegulationOrder, int currentTraOwner);

    void SetSourceActionType(ref Models.DataBase.DigitalTrafficRegulationOrder digitalTrafficRegulationOrder, SourceActionType sourceActionType);
}
