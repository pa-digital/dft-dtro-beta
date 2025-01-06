namespace DfT.DTRO.Enums;

/// <summary>
/// Indicates linear direction type
/// </summary>
public enum LinearDirectionType
{
    /// <summary>
    /// Bidirectional linear direction
    /// </summary>
    [Display(Name = "bidirectional")]
    Bidirectional = 1,

    /// <summary>
    /// Start to end linear direction
    /// </summary>
    [Display(Name = "startToEnd")]
    StartToEnd = 2,

    /// <summary>
    /// End to start linear direction
    /// </summary>
    [Display(Name = "endToStart")]
    EndToStart = 3
}