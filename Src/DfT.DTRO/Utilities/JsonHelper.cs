using Newtonsoft.Json;

namespace DfT.DTRO.Utilities;

public static class JsonHelper
{
    public static TTarget ConvertObject<TSource, TTarget>(TSource source)
    {
        var json = JsonConvert.SerializeObject(source);
        return JsonConvert.DeserializeObject<TTarget>(json);
    }
}