namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for user
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// User forename
    /// </summary>
    public string Forename { get; set; }

    /// <summary>
    /// User surname
    /// </summary>
    public string Surname { get; set; }

    /// <summary>
    /// User email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// User status related to user
    /// </summary>
    public Guid UserStatusId { get; set; }

    /// <summary>
    /// Digital service provider related to user
    /// </summary>
    public List<DigitalServiceProvider> DigitalServiceProviders { get; set; }

    /// <summary>
    /// User is central service operator
    /// </summary>
    public bool IsCentralServiceOperator { get; set; }
}