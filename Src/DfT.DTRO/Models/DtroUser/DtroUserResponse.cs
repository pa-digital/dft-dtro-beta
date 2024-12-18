namespace DfT.DTRO.Models.SwaCode;

/// <summary>
/// D-TRO User data transfer object response
/// </summary>
public class DtroUserResponse
{
    /// <summary>
    /// Unique identification
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// x-app-id header unique identifier
    /// </summary>
    public Guid xAppId { get; set; }

    /// <summary>
    /// Traffic regulation authority code
    /// </summary>
    public int? TraId { get; set; }

    /// <summary>
    /// User name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// User prefix
    /// </summary>
    public string Prefix { get; set; }

    /// <summary>
    /// User group
    /// </summary>
    public UserGroup UserGroup { get; set; }
}