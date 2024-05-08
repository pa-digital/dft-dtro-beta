using NpgsqlTypes;
using System;

namespace DfT.DTRO.DAL;

/// <summary>
/// Methods to help work with the database.
/// </summary>
public static class DatabaseMethods
{
    /// <summary>
    /// Checks if the boxes overlap.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>True if boxes overlap.</returns>
    public static bool Overlaps(NpgsqlBox left, NpgsqlBox right)
    {
        throw new InvalidOperationException("This does not have an in-program implementation.");
    }
}
