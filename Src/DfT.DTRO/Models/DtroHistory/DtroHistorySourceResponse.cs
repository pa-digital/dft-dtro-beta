using System;
using DfT.DTRO.Models.SchemaTemplate;

namespace DfT.DTRO.Models.DtroHistory;

public class DtroHistorySourceResponse
{
    public string ActionType { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? LastUpdated { get; set; }

    public string Reference { get; set; }

    public SchemaVersion SchemaVersion { get; set; }

    public string Section { get; set; }

    public int TrafficAuthorityCreatorId { get; set; }

    public int TrafficAuthorityOwnerId { get; set; }

    public string TroName { get; set; }
}