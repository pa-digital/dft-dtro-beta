namespace DfT.DTRO.Models.Validation;

/// <summary>
/// Data structure providing explanation for bounding box validation error.
/// </summary>
public class BoundingBoxErrors
{
    /// <summary>
    /// Gets longitude related error explanation.
    /// </summary>
    public string LongitudeError { get; init; }

    /// <summary>
    /// Gets latitude related error explanation.
    /// </summary>
    public string LatitudeError { get; init; }
}