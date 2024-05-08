namespace DfT.DTRO.Models.DtroEvent;

/// <summary>
/// Type of the DTRO change event.
/// </summary>
public enum DtroEventType
{
    /// <summary>
    /// A creation event.
    /// </summary>
    Create,

    /// <summary>
    /// An update event.
    /// </summary>
    Update,

    /// <summary>
    /// A soft-deletion event.
    /// </summary>
    Delete
}