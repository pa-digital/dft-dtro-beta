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
    /// List of Traffic Regulation Authority related to the Application
    /// </summary>
    public List<TrafficRegulationAuthority> TrafficRegulationAuthorities { get; set; }

    /// <summary>
    /// List of Digital Service Provider related to the Application
    /// </summary>
    public List<DigitalServiceProvider> DigitalServiceProviders { get; set; }

    /// <summary>
    /// Application type related to the Application
    /// </summary>
    public ApplicationType ApplicationType { get; set; }

    /// <summary>
    /// Application purpose related to the Application
    /// </summary>
    public ApplicationPurpose ApplicationPurpose { get; set; }
}