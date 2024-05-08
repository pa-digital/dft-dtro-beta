using System;

namespace DfT.DTRO.Models.SharedResponse;

/// <summary>
/// A confirmation of storage paired with an accompanying ID.
/// </summary>
public class GuidResponse
{
    /// <summary>
    /// The unique identifier of the record stored centrally.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public GuidResponse()
    {
        Id = Guid.NewGuid();
    }
}