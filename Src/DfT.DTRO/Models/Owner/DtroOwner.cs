namespace DfT.DTRO.Models.Owner;

/// <summary>
/// D-TRO owner object to request.
/// </summary>
public class DtroOwner
{
    /// <summary>
    /// Traffic regulation authority creator ID.
    /// </summary>
    public int TrafficAuthorityCreatorId { get; set; }

    /// <summary>
    /// Traffic regulation authority owner ID.
    /// </summary>
    public int TrafficAuthorityOwnerId { get; set; }
}