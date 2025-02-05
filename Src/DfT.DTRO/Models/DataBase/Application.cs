namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for Application
/// </summary>
public class Application : BaseEntity
{
    /// <summary>
    /// Automatic generated name of the Application
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// Traffic Regulation Authority unique identifier related to the Application
    /// </summary>
    public Guid TrafficRegulationAuthorityId { get; set; }

    /// <summary>
    /// Digital Service Provider unique identifier related to the Application
    /// </summary>
    public Guid DigitalServiceProviderId { get; set; }

    /// <summary>
    /// Application Type unique identifier related to the Application
    /// </summary>
    public Guid ApplicationTypeId { get; set; }

    /// <summary>
    /// Application Purpose unique identifier related to the Application
    /// </summary>
    public Guid ApplicationPurposeId { get; set; }
}