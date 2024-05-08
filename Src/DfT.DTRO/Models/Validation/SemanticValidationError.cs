namespace DfT.DTRO.Models.Validation;

/// <summary>
/// Model for capturing semantic validation errors.
/// </summary>
public class SemanticValidationError
{
    /// <summary>
    /// The message detail of the error encountered.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// The path in the JSON object at fault.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// The type of error encountered (always semantic for this class).
    /// </summary>
    public string ErrorType
    {
        get { return "semantic"; }
    }
}