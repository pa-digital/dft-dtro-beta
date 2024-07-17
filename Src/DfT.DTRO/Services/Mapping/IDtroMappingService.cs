using DfT.DTRO.Enums;
using DfT.DTRO.Models.DtroEvent;
using DfT.DTRO.Models.DtroHistory;
using DfT.DTRO.Models.Search;

namespace DfT.DTRO.Services.Mapping;

public interface IDtroMappingService
{
    void InferIndexFields(ref Models.DataBase.DTRO dtro);

    DtroResponse MapToDtroResponse(Models.DataBase.DTRO dtro);

    IEnumerable<DtroEvent> MapToEvents(IEnumerable<Models.DataBase.DTRO> dtros);

    IEnumerable<DtroSearchResult> MapToSearchResult(IEnumerable<Models.DataBase.DTRO> dtros);

    DtroHistorySourceResponse GetSource(DTROHistory dtroHistory);

    List<DtroHistoryProvisionResponse> GetProvision(DTROHistory dtroHistory);

    DTROHistory MapToDtroHistory(Models.DataBase.DTRO currentDtro);

    DtroOwner GetOwnership(Models.DataBase.DTRO dtro);

    void SetOwnership(ref Models.DataBase.DTRO dtro, int currentTraOwner);

    void SetSourceActionType(ref Models.DataBase.DTRO dtro, SourceActionType sourceActionType);
}
