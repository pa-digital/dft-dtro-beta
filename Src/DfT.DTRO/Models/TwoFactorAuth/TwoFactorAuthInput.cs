namespace DfT.DTRO.Models.TwoFactorAuth;

/// <summary>
/// Two factor auth input
/// </summary>
[DataContract]
public class TwoFactorAuthInput
{
    /// <summary>
    /// Token
    /// </summary>
    [DataMember(Name = "token")]
    public string Token { get; set; }

    /// <summary>
    /// Code
    /// </summary>
    [DataMember(Name = "code")]
    public string Code { get; set; }
}