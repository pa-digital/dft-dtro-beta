using System.Collections.Generic;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.DtroEvent;
using DfT.DTRO.Models.Search;

namespace DfT.DTRO.Services.Mapping;

/// <summary>
/// Provides methods used for mapping <see cref="Models.DataBase.DTRO"/> to other types.
/// </summary>
public interface IDtroMappingService
{
    /// <summary>
    /// Infers the fields that are not directly sent in the request
    /// but are used in the database for search optimization.
    /// </summary>
    /// <param name="dtro">The <see cref="Models.DataBase.DTRO"/> to infer index fields for.</param>
    void InferIndexFields(ref Models.DataBase.DTRO dtro);

    DtroResponse MapToDtroResponse(Models.DataBase.DTRO dtro);

    IEnumerable<DtroEvent> MapToEvents(IEnumerable<Models.DataBase.DTRO> dtros);

    IEnumerable<DtroSearchResult> MapToSearchResult(IEnumerable<Models.DataBase.DTRO> dtros);

    DTROHistory AsHistoryDtro(Models.DataBase.DTRO currentDtro);

    void UpdateDetails(Models.DataBase.DTRO currentDtro, DtroSubmit dtroSubmit);

    List<DTROHistory> StripProvisions(List<DTROHistory> dtroHistories);
}
