namespace DfT.DTRO.Helpers;

/// <summary>
/// Application status type
/// </summary>
public static class ApplicationStatusType
{
    /// <summary>
    /// Active status
    /// </summary>
    public static (Guid Id, string Status) Active = (Guid.Parse("6e563a53-05b6-4521-8323-896aecc27cc1"), "Active");

    /// <summary>
    /// Pending status
    /// </summary>
    public static (Guid Id, string Status) Pending = (Guid.Parse("3153c4a8-6434-46bf-99ff-98500c02e983"), "Pending");
    
    /// <summary>
    /// Inactive status
    /// </summary>
    public static (Guid Id, string Status) Inactive = (Guid.Parse("c47cc91d-0d20-47b0-8fc8-ec51fb5aae94"), "Inactive");
}