﻿using System.Collections.Generic;
using DfT.DTRO.Enums;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.DtroEvent;
using DfT.DTRO.Models.DtroHistory;
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

    DtroHistorySourceResponse GetSource(DTROHistory dtroHistory);

    List<DtroHistoryProvisionResponse> GetProvision(DTROHistory dtroHistory);

    DTROHistory MapToDtroHistory(Models.DataBase.DTRO currentDtro);

    DtroOwner GetOwnership(Models.DataBase.DTRO dtro);

    void SetOwnership(ref Models.DataBase.DTRO dtro, int currentTraOwner);

    void SetSourceActionType(ref Models.DataBase.DTRO dtro, SourceActionType sourceActionType);
}
