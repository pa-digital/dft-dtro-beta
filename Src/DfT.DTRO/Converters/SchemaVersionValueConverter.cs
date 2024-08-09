namespace DfT.DTRO.Converters;

/// <summary>
/// Defines conversion between a <see cref="SchemaVersionValueConverter"/> object
/// and its string representation in the database.
/// </summary>
public class SchemaVersionValueConverter : ValueConverter<SchemaVersion, string>
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public SchemaVersionValueConverter()
        : base(
            schema => schema.ToString(),
            databaseValue => new SchemaVersion(databaseValue))
    {
    }
}
