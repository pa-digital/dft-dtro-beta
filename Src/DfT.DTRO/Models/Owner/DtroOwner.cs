using System;
using DfT.DTRO.Models.SchemaTemplate;

namespace DfT.DTRO.Models.DtroHistory;

public class DtroOwner
{
    public int TrafficAuthorityCreatorId { get; set; }

    public int TrafficAuthorityOwnerId { get; set; }
}