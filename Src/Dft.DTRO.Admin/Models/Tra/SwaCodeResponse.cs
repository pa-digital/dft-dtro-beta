/// <summary>
/// Swa codes response
/// </summary>
public class SwaCodeResponse
{
    /// <summary>
    /// Gets or sets the TRA id of the request.
    /// </summary>
    public int TraId { get; set; }

    /// <summary>
    /// Gets or sets the TRA name of the request.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the TRA prefix of the request.
    /// </summary>
    public string Prefix { get; set; }

    /// <summary>
    /// Gets or sets the TRA as administrator.
    /// </summary>
    public bool IsAdmin { get; set; }
}