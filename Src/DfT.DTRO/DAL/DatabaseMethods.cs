namespace DfT.DTRO.DAL;

/// <summary>
/// Extension class for database methods.
/// </summary>
public static class DatabaseMethods
{
    /// <summary>
    /// Check if the boxes overlaps.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>True if the boxes overlap.</returns>
    /// <exception cref="InvalidOperationException">Exception not implemented.</exception>
    public static bool Overlaps(NpgsqlBox left, NpgsqlBox right)
    {
        throw new InvalidOperationException("This does not have an in-program implementation.");
    }
}
