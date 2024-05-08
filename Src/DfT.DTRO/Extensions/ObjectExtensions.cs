using Newtonsoft.Json;

namespace DfT.DTRO.Extensions;
public static class ObjectExtensions
{
    public static string ToIndentedJsonString(this object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }
}