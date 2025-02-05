namespace DfT.DTRO.Services.Mapping;

public interface IDtroMappingService
{
    void InferIndexFields(ref Models.DataBase.DTRO dtro);

    DtroResponse MapToDtroResponse(Models.DataBase.DTRO dtro);

    IEnumerable<DtroEvent> MapToEvents(IEnumerable<Models.DataBase.DTRO> dtros, DateTime? searchSince);

    IEnumerable<DtroSearchResult> MapToSearchResult(IEnumerable<Models.DataBase.DTRO> dtros);

    DtroHistorySourceResponse GetSource(DTROHistory dtroHistory);

    List<DtroHistoryProvisionResponse> GetProvision(DTROHistory dtroHistory);

    DTROHistory MapToDtroHistory(Models.DataBase.DTRO currentDtro);

    DtroOwner GetOwnership(Models.DataBase.DTRO dtro);

    void SetOwnership(ref Models.DataBase.DTRO dtro, int currentTraOwner);

    void SetSourceActionType(ref Models.DataBase.DTRO dtro, SourceActionType sourceActionType);
}
