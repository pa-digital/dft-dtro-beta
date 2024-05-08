using DfT.DTRO.Models.SchemaTemplate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DfT.DTRO.Converters;

/// <summary>
/// Defines conversion between a <see cref="SchemaVersion"/> object and its string representation in the database.
/// </summary>
public class SchemaVersionValueConverter : ValueConverter<SchemaVersion, string>
{
    /// <summary>
    /// The single constructor.
    /// </summary>
    public SchemaVersionValueConverter()
        : base(
            schema => schema.ToString(),
            databaseValue => new SchemaVersion(databaseValue))
    {
    }
}
