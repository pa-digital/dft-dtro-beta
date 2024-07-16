using DfT.DTRO.Models.SchemaTemplate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DfT.DTRO.Converters;

public class SchemaVersionValueConverter : ValueConverter<SchemaVersion, string>
{
    public SchemaVersionValueConverter()
        : base(
            schema => schema.ToString(),
            databaseValue => new SchemaVersion(databaseValue))
    {
    }
}
