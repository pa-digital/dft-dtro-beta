using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System.Dynamic;

namespace DfT.DTRO.Converters;

/// <summary>
/// Defines conversion between an <see cref="ExpandoObject"/> and its string representation in the database.
/// </summary>
public class ExpandoObjectValueConverter : ValueConverter<ExpandoObject, string>
{
    /// <summary>
    /// The single constructor.
    /// </summary>
    public ExpandoObjectValueConverter()
        : base(
        expando => JsonConvert.SerializeObject(expando),
        databaseValue => JsonConvert.DeserializeObject<ExpandoObject>(databaseValue))
    {
    }
}
