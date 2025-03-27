namespace DfT.DTRO.Enums;

/// <summary>
/// List of vehicle usage types
/// </summary>
public enum VehicleUsageType
{
    /// <summary>
    /// Access usage type
    /// </summary>
    [Display(Name = "access")]
    Access,

    /// <summary>
    /// Access To Off Street Premises usage type
    /// </summary>
    [Display(Name = "accessToOffStreetPremises")]
    AccessToOffStreetPremises,
    
    /// <summary>
    /// Authorised Vehicle usage type
    /// </summary>
    [Display(Name = "authorisedVehicle")]
    AuthorisedVehicle,
    
    /// <summary>
    /// Guided Buses usage type
    /// </summary>
    [Display(Name = "guidedBuses")]
    GuidedBuses,
    
    /// <summary>
    /// Highway Authority Purpose usage type
    /// </summary>
    [Display(Name = "highwayAuthorityPurpose")]
    HighwayAuthorityPurpose,
    
    /// <summary>
    /// Local Buses usage type
    /// </summary>
    [Display(Name = "localBuses")]
    LocalBuses,
    
    /// <summary>
    /// Local Registered Private Hire Vehicle usage type
    /// </summary>
    [Display(Name = "localRegisteredPrivateHireVehicle")]
    LocalRegisteredPrivateHireVehicle,
    
    /// <summary>
    /// Private Hire Vehicle usage type
    /// </summary>
    [Display(Name = "privateHireVehicle")]
    PrivateHireVehicle,
    
    /// <summary>
    /// Bus Operation Purpose usage type
    /// </summary>
    [Display(Name = "busOperationPurpose")]
    BusOperationPurpose,
    
    /// <summary>
    /// Statutory Undertaker Purpose usage type
    /// </summary>
    [Display(Name = "statutoryUndertakerPurpose")]
    StatutoryUndertakerPurpose,
    
    /// <summary>
    /// Military usage type
    /// </summary>
    [Display(Name = "military")]
    Military,
    
    /// <summary>
    /// Other usage type
    /// </summary>
    [Display(Name = "other")]
    Other
}