using System.Dynamic;
using System;
using DfT.DTRO.Models.SchemaTemplate;

namespace DfT.DTRO.Models.DtroHistory;

public class DtroHistoryResponse
{
    public Guid Id { get; set; }

    public Guid DtroId { get; set; }

    public SchemaVersion SchemaVersion { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? LastUpdated { get; set; }

    public bool Deleted { get; set; }

    public DateTime? DeletionTime { get; set; }

    public ExpandoObject Data { get; set; }

    public int TrafficAuthorityCreatorId { get; set; }

    public int TrafficAuthorityOwnerId { get; set; }
}