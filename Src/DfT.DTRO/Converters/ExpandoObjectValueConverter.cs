using System.Dynamic;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace DfT.DTRO.Converters;

public class ExpandoObjectValueConverter : ValueConverter<ExpandoObject, string>
{
    public ExpandoObjectValueConverter()
        : base(
        expando => JsonConvert.SerializeObject(expando),
        databaseValue => JsonConvert.DeserializeObject<ExpandoObject>(databaseValue))
    {
    }
}
