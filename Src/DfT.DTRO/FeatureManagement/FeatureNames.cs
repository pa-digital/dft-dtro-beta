namespace DfT.DTRO.FeatureManagement;

/// <summary>
/// Contains feature names as constants to use instead of string literals.
/// </summary>
public static class FeatureNames
{
    /// <summary>
    /// Allows reading DTRO data.
    /// </summary>
    public const string DtroRead = "DtroRead";

    /// <summary>
    /// Allows creating and updating DTROs.
    /// </summary>
    public const string DtroWrite = "DtroWrite";

    /// <summary>
    /// Allows getting information about schemas.
    /// </summary>
    public const string SchemasRead = "SchemasRead";

    /// <summary>
    /// Allows creating and updating Schemas.
    /// </summary>
    public const string SchemaWrite = "SchemaWrite";
}
