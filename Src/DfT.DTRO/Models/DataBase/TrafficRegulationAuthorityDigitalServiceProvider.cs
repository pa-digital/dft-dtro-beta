namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for Traffic Regulation Authority and Digital Service Provider
/// </summary>
public class TrafficRegulationAuthorityDigitalServiceProvider : BaseEntity
{
    /// <summary>
    /// List of traffic regulation authorities
    /// </summary>
    public List<TrafficRegulationAuthority> TrafficRegulationAuthorities { get; set; }


    /// <summary>
    /// List of digital service provider
    /// </summary>
    public List<DigitalServiceProvider> DigitalServiceProviders { get; set; }

    /// <summary>
    /// Relationship status between Traffic Regulation Authority and Digital Service Provider
    /// </summary>
    public TrafficRegulationAuthorityDigitalServiceProviderStatus TrafficRegulationAuthorityDigitalServiceProviderStatus { get; set; }
}